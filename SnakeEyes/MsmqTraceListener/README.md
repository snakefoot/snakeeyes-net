# MsmqTraceListener

This listener sends a trace event to a message queue.

Config options:

| Key             | Explaination                                                             | Default |
| --------------- | ------------------------------------------------------------------------ | ------- |
| queueName       | Name of the queue to send the traces to                                  |         |
| queueLabel      | The label the queue should have                                          |         |
| formatBody      | The format of the body.                                                  |         |
| formatLabel     | The label the queue should have                                          |         |

Both `formatBody` and `formatLabel` are processed by a method that replaces `{{xpath}}` with the value of the XPATH applied on the TraceEvent message.
If `formatBody` is not given the original trace message will be submitted.

Here is a minimal configuration for the SnakeEyesService that sends the amount of free ram to the queue `SnakeEyesInbox` on the local computer:

```xml
<?xml version="1.0"?>
<configuration>
	<configSections>
		<section name="TriggerEveryHour.Filter" type="System.Configuration.NameValueSectionHandler"/>
		<section name="RAM_FreeBytes.PerfMonProbe" type="System.Configuration.NameValueSectionHandler"/>
	</configSections>
	<startup>
		<supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5"/>
	</startup>
	<TriggerEveryHour.Filter>
		<add key="NextTriggerTime" value="3600"/>
		<add key="DelayTriggerTime" value="0"/>
	</TriggerEveryHour.Filter>
	<RAM_FreeBytes.PerfMonProbe>
		<add key="CategoryName" value="Memory"/>
		<add key="CounterName" value="Available MBytes"/>
		<add key="MinValue" value="1024"/>
		<add key="EventId" value="1"/>
		<add key="EventType" value="Warning"/>
		<add key="ProbeFrequency" value="5"/>
	</RAM_FreeBytes.PerfMonProbe>
	<system.diagnostics>
		<sources>
			<source name="RAM_FreeBytes.PerfMonProbe">
				<listeners>
					<clear/>
					<add name="MsmqEvents"/>
				</listeners>
			</source>
		</sources>
		<sharedListeners>
			<add type="SnakeEyes.MsmqTraceListener, MsmqTraceListener" name="MsmqEvents" queueName=".\Private$\SnakeEyesInbox" queueLabel="Snake Eyes Inbox">
			</add>
		</sharedListeners>
	</system.diagnostics>
</configuration>
```
