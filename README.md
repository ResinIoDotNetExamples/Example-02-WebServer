# A Simple .Net-based Web Server

## Introduction

This repository contains the project files, source code and compiled binaries
for a basic web server that can be deployed through Resin.io.

This is not an example on how to build web-servers, but rather on how to
add web-service features to some other project. For instance, if you
had a temperature logger and wanted to be able to respond to web requests
and return the current temperature.

# Requirements

* Raspberry Pi or Raspberry Pi 2 with power, networking, and an SD card
* An installation of mono-complete on Windows, OSX, or Linux

# Resin.io Setup

1. If you haven't got a [Resin.io](http://resin.io) account, visit
[resin.io](http://resin.io) and sign up.
1. Start a new applicaton on [Resin.io](http://resin.io) and follow the
directions for your target device type to load the image onto your SD card.
1. Insert the SD card into the Raspberry Pi, power it up and wait for it
to connect to the internet and resin.io.
1. After about 10 minutes your new device should show up on your application 
dashboard.

## Running the example code

You can deploy the unaltered example code just to see it run on your device.
To do so, follow these steps...

1. Clone the WebServer repo:

```sh
$ git clone https://github.com/ResinIoDotNetExamples/Example-02-WebServer.git
```

Add the resin remote. You can find an example of this command and 
and a copyable link with your username and application name by clicking
on your application on the resin.io dashboard. The information you need
is at the top right next to the Need Help? button:

```sh
$ git remote add resin git@git.resin.io:<yourUserName>/<yourApplicationName>.git
```

And finally push the code to your Raspberry Pi:

`$ git push resin master`

After deployment is complete, go to the resin.io dashboard; select
your application; select your test device; and click on "Logs". At the end of
the deployment process, you should see log entries showing that your web
server has started up and showing that three 'content services' have been
installed.

## Testing

1. On the resin.io dashboard, click on your app.
1. Click on your device
1. Note the IP address
1. Open a web browser and enter http://XX.XX.XX.XX:8080/ replacing the XX's
with the IP address from the previous step.
1. You should see a "Hello World" message.
1. Now try http://XX.XX.XX.XX:8080/time and you should see the current UTC
time.

## Reproducing the sample from scratch.

The process is the same as for Example-01, except that you also need to
choose "Mono / .NET 4.5" as the target runtime.  To do so, right click
on the project and select Options. Under Build, select "General" and
select "Mono / .NET 4.5" as the target runtime. This will enable use
of the .Net 4.5 async semantics and Task-based scheduling, which makes
much more efficient use of your CPU resources.



