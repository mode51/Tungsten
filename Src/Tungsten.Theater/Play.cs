using System;
using System.Threading;
using W.Threading;

namespace W.Threater
{
    public sealed class Play<TTheater> : IDisposable
    {
        private ThreadMethod _thread;
        private TTheater _theater;
        private ManualResetEventSlim _isActive = new ManualResetEventSlim(true);
        private System.Collections.Concurrent.ConcurrentQueue<SceneResult<TTheater>> Actors { get; } = new System.Collections.Concurrent.ConcurrentQueue<SceneResult<TTheater>>();

        public event Action<Play<TTheater>, SceneResult<TTheater>> SceneCompleted;// = new EventTemplate<Play<TTheater>, SceneResult<TTheater>>();
        public CPUProfileEnum ThreadingProfile { get; set; } = CPUProfileEnum.SpinWait1;

        public SceneResult<TTheater> StageScene(Scene<TTheater> scene, params object[] args)
        {
            var result = new SceneResult<TTheater>() { Scene = scene.Clone() };
            result.Scene.Args = args;
            //Scripts.InLock(list =>
            //{
            //    list.RemoveAll(i => i.Name == scene.Name);
            //    list.Add(scene);
            //});

            Actors.Enqueue(result);
            return result;
        }
        public SceneResult<TTheater> StageScene(string name, Action<TTheater, SceneResult<TTheater>> action, params object[] args)
        {
            var scene = new Scene<TTheater>() { Name = name, Action = action };
            return StageScene(scene, args);
        }
        public void Pause()
        {
            _isActive.Reset();
        }
        public void Resume()
        {
            _isActive.Set();
        }
        public void Dispose()
        {
            Pause();
            while (Actors.Count > 0)
                Actors.TryDequeue(out SceneResult<TTheater> result);
            Resume();//have to resume to exit the threadproc upon cancellation
            _thread.Cancel();
            _thread.Dispose();
            _isActive.Dispose();
        }
        public Play(TTheater theater)
        {
            _theater = theater;
            _thread = new ThreadMethod(token =>
            {
                while (!token.IsCancellationRequested)
                {
                    _isActive.Wait();
                    if (Actors.Count > 0)
                    {
                        if (Actors.TryDequeue(out SceneResult<TTheater> sceneResult))
                        {
                            try
                            {
                                sceneResult.IsComplete.Reset();
                                sceneResult.Scene.Action.Invoke(_theater, sceneResult);
                            }
                            catch (Exception e)
                            {
                                sceneResult.Exception = e;
                            }
                            finally
                            {
                                sceneResult.IsComplete.Set();
                            }
                            SceneCompleted?.Invoke(this, sceneResult);
                        }
                    }
                    W.Threading.Thread.Sleep(ThreadingProfile);
                }
            });
            _thread.Start();
        }
    }
}
