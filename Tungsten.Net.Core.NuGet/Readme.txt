Tungsten.Net.Core is a Tcp socket library.  Clients and servers are provided; you can use assymetric encryption or not.

2.27.2017 - Initial Release
SecureStringServer - extends SecureServer to send and receive encrypted strings
SecureStringClient - extends StringClient with assymetric encryption (must be used with SecureStringServer)
StringServer - a server which sends and receives strings
StringClient - extends FormattedSocket which sends and receives text instead of byte[]

SecureServer - servers as a base class for a server which sends and receives encrypted strings (uses assymetric encryption)
Server - serves as a base class for StringServer and SecureStringServer (or a server you wish to implement)
FormattedSocket - a base class proxy to Socket which allows the programmer to customize sent and received data
Socket - the TcpClient wrapper which handles sending and receiving data as byte arrays


Thanks for using Tungsten.Net.Core

Jordan Duerksen


