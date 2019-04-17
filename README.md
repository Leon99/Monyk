# Monyk

[![Build Status](https://dev.azure.com/monyk/Monyk/_apis/build/status/Leon99.Monyk?branchName=master)](https://dev.azure.com/monyk/Monyk/_build/latest?definitionId=1&branchName=master)

## Overview

### What is this?

It's a distributed reliability monitoring tool that can be operated without learning rocket science.
It's like UptimeRobot, only open-source, extensible, self-hosted, cross-platform, RESTful, DevOps-friendly and cost-efficient.

### What exactly can it do?

At the moment, it can check HTTP endpoints and ping hosts from multiple locations.
Failure alerts are sent to Slack.

### Anything else I need to know before trying it?

Monyk consist of the following loosely-coupled components:

1. *GroundControl* allows you to specify *monitors* via its REST API and will take care of triggering the *checks* according to the specifications.
2. *Probes* actually perform the checks. You may want to put many of those around the globe - it's optimized for minimal resource consumption.
3. *Lab* processes the results of the checks.

### OK how do I use it?

Note that all the further commands assume that you have cloned the repository:

```Shell
git clone https://github.com/leon99/monyk
cd monyk
```

#### Run using Docker Compose

`docker-compose up`

#### Run manually

1. [Install RabbitMQ](https://www.rabbitmq.com/download.html)
2. Make sure that you have the latest release of [.NET Core SDK](https://dotnet.microsoft.com/download) installed
3. Run *GroundControl*: `dotnet run --project .\src\Monyk.GroundControl.Main`
4. Run *Probes*: `dotnet run --project .\src\Monyk.Lab.Main`
5. Run *Lab*: `dotnet run --project .\src\Monyk.Probe.Main`
6. (Optional) [Install Seq](https://docs.getseq.net/docs/getting-started-with-docker) to make use of semantic logging support

Either method will launch Monyk using default setting for development environment:

- database re-created each time *GroundControl* starts
- a few monitors initially added to help kick-start things
- *GroundControl* API accessible from browser via https://localhost:42011/swagger
- *Lab* is not configured to do any result processing
- all the components are unsecured

See [Configuration](#configuration) for details on preparing it to run in the wild.

#### Hold on, but you said there will be no rocket science?

Feel free to improve things and submit a PR 🖖

## Details

### Configuration

Configuration can be done using any of the following methods (in order of priority, starting with the highest):

1. Command line parameters
2. Environment variables
3. Using `appsettings.<environment>.yml`, when `<environment>` can be specified using `ASPNETCORE_ENVIRONMENT` environment variable

Check `appsettings-reference.yml` for the reference on available settings.

When configuring using environment variables, use `MONYK_` prefix and `__` as a delimiter in hierarchical keys. Examples can be found in `docker-compose.override.yml`.