# Monyk

## Overview

### What is this?

It's a distributed reliability monitoring tool that can be operated without learning rocket science.
It's like UptimeRobot, only open-source, self-hosted, cross-platform, DevOps-friendly and cost-efficient.

### What exactly can it do?

At the moment, it can monitor web sites and ping hosts from multiple locations.
Monitoring results can be stored in [Graphite](https://graphiteapp.org).

### OK how do I use it?

1. [Build Monyk](#building-from-source).
2. [Install RabbitMQ](https://www.rabbitmq.com/download.html).
3. Run *GroundControl*: `dotnet bin/GroundControl/Monyk.GroundControl.Main.dll`. It'll allow you to specify *monitors* via its REST API and will take care of triggering the *checks* according to the specifications.
4. Run *Probes*: `dotnet bin/Probe/Monyk.Probe.Main.dll`. They'll be actually performing the checks. You may want to put many of those around the globe. Go to http://localhost:42011/swagger to try it.
5. Run *Lab*: `dotnet bin/Lab/Monyk.Lab.Main.dll`. It'll be processing the results of the checks.

#### Hold on, but you said there will be no rocket science?

I lied. It won't happen again, I promise 🖖

## Details

### Building from Source

First, make sure that you have the latest release of [.NET Core SDK](https://dotnet.microsoft.com/download) installed.

```Shell
git clone https://github.com/leon99/monyk
cd monyk
dotnet build src/Monyk.GroundControl.Main --output ../../bin/GroundControl
dotnet build src/Monyk.Probe.Main --output ../../bin/Probe
dotnet build src/Monyk.Lab.Main --output ../../bin/Lab
```