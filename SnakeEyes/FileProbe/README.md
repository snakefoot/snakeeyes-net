# FileProbe

This prober checks if a file does not exist or is not bigger than MaxSize bytes. This can be used to see if error logs are used.

Config options:

| Key             | Explaination                                                             | Default |
| --------------- | ------------------------------------------------------------------------ | ------- |
| FileName        | Name of the file to                                                      |         |
| MaxFileSize     | Max size (bytes) that the file is allowed to have                        | null    |
| MaxFileAge      | Max age (sec) since the last modification (mtime)                        | null    |
| DefaultFileSize | Default value for file size in case the file does not exist              | null    |
| DefaultFileAge  | Default value for time since last change in case the file does not exist | null    |
| ProbeFrequency  | How often shall SnakeEyes execute this test (sec)                        | 5       |
| EventId         | ID of the event                                                          |         |
| EventType       | Event type (of type TraceEventType)                                      | Warning |

Note will generate a Critical TraceEvent if if both DefaultFileSize and DefaultFileAge are not set and the file does not exist.

Here is a minimal configuration for the SnakeEyesClient that checks if the file `C:\Error.log` is empty or does not exist:

```xml
<?xml version="1.0"?>
<configuration>
  <configSections>
    <section name="TriggerEveryHour.Filter" type="System.Configuration.NameValueSectionHandler"/>
    <section name="ErrorLog.FileProbe" type="System.Configuration.NameValueSectionHandler"/>
  </configSections>
    <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
  </startup>
  <TriggerEveryHour.Filter>
    <add key="NextTriggerTime" value="3600"/>
    <add key="DelayTriggerTime" value="0"/>
  </TriggerEveryHour.Filter>
  <ErrorLog.FileProbe>
    <add key="FileName" value="C:\Error.log"/>
    <add key="MaxFileSize" value="0"/>
	<add key="DefaultFileSize" value="0"/>
    <add key="EventId" value="30"/>
    <add key="EventType" value="Warning"/>
    <add key="ProbeFrequency" value="10"/>
  </ErrorLog.FileProbe>
  <system.diagnostics>
    <sources>
      <source name="ErrorLog.FileProbe" switchValue="Information">
        <listeners>
          <clear/>
        </listeners>
      </source>
    </sources>
  </system.diagnostics>
</configuration>
```

