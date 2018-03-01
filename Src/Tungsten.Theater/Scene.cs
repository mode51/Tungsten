using System;

namespace W.Threater
{
    public class Scene<TTheater>
    {
        public string Name { get; set; }
        public Action<TTheater, SceneResult<TTheater>> Action { get; set; }
        public object[] Args { get; set; }
        public T GetArg<T>(int index) { return (T)Args[index]; }
        public Scene<TTheater> Clone() { return new Scene<TTheater>() { Name = Name, Action = Action, Args = Args }; }
    }
}
