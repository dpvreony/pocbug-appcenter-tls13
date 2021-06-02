# pocbug-appcenter-tls13
Proof of concept of issue in AppCenter for Windows client + TLS1.3 as of 2021-06-02

This is a crude reproduction for https://github.com/microsoft/appcenter-sdk-dotnet/issues/1531 where a change to the default behaviour on the
servicepoint manager on net47 onwards ends up with tls 1.2 being the ONLY enabled transport security.

A more effective description of the issue is "any non tls1.2 transport will not work once appcenter is enabled on windows clients" right now this is limited as most things should be using TLS1.2 but it does mean that Appcenter isn't forward compatible for new protocols enabled by the OS. But this is an edge case at this point in time. The bigger issue would actually be that it breaks TLS1.1 on net47 onward.

This concept uses TLS 1.1 in the example as enabling TLS 1.3 requires changes in the registry. https://stackoverflow.com/questions/56072561/how-to-enable-tls-1-3-in-windows-10

TLS1.1 as a sample still shows the issue with the OR operation on the ServicePointManager
```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Client]
"DisabledByDefault"=dword:00000000 
"Enabled"=dword:00000001
```

```
Windows Registry Editor Version 5.00

[HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\SecurityProviders\SCHANNEL\Protocols\TLS 1.3\Server]
"DisabledByDefault"=dword:00000000 
"Enabled"=dword:00000001
```

The logic is in the OnStartup of the WPF app. It will need the AppCenter startup uncommenting with an AppKey guid.

