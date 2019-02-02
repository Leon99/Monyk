# Monyk

## Overview

### What is this?

It's a distributed reliability monitoring tool that can be operated without learning rocket science.
It's like UptimeRobot, only open-source, extensible, self-hosted, cross-platform, RESTful, DevOps-friendly and cost-efficient.

### What exactly can it do?

At the moment, it can monitor web sites and ping hosts from multiple locations.
Failure alerts are sent to Slack.

### Anything else I need to know before trying it?

Monyk consist of the following loosely-coupled components:

1. *GroundControl* allows you to specify *monitors* via its REST API and will take care of triggering the *checks* according to the specifications.  
2. *Probes* actually perform the checks. You may want to put many of those around the globe.
3. *Lab* processes the results of the checks.

### OK how do I use it?

Note that all the further commands assume that you have cloned the repository:

```Shell
git clone https://github.com/leon99/monyk
cd monyk
```

#### Run using Docker Compose:
`docker-compose up`
#### Run manually:

1. [Install RabbitMQ](https://www.rabbitmq.com/download.html)
2. Make sure that you have the latest release of [.NET Core SDK](https://dotnet.microsoft.com/download) installed
3. Run *GroundControl*: `dotnet run --project .\src\Monyk.GroundControl.Main`
4. Run *Probes*: `dotnet run --project .\src\Monyk.Lab.Main`
5. Run *Lab*: `dotnet run --project .\src\Monyk.Probe.Main`

This will launch Monyk using default setting for development environment:

- a few monitors initially added to help kick-start things
- *GroundControl* API accessible from browser via https://localhost:42011/swagger
- an internal Sqlite database re-created each time *GroundControl* starts. Using [PostgreSQL](https://www.postgresql.org) is recommended for production scenarios.

See [Configuration](#configuration) for details on preparing it to run in the wild.
#### Hold on, but you said there will be no rocket science?

Feel free to correct it and submit a PR 🖖

## Details

### Configuration

Configuration can be done using any of the following methods (in order of priority, starting with the highest):
1. command line parameters;
2. environment variables;
3. using `appsettings.<environment>.yml`, when `<environment>` can be specified using `ASPNETCORE_ENVIRONMENT` environment variable.

Check `appsettings.yml` for the reference on available settings.

When configuring using environment variables, `__` can be used as a delimiter in hierarchical keys. Example:
```yaml
Monyk.Common.Communicator:
  RabbitMQ:
    Host: 
```
can be set using an environment variable `Monyk.Common.Communicator__RabbitMQ__Host`.