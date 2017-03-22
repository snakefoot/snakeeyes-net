# FileProbe

This prober checks if a file does not exist or is not bigger than MaxSize bytes. This can be used to see if error logs are used.

Here is a minimal configuration for the SnakeEyesClient:

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
    <add key="FileName" value="G:\Error.log"/>
    <add key="MaxFileSize" value="0"/>
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

