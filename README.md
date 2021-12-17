# NetworkAdapterRestartService

Windows service for restarting ethernet newtwork adapters. Easily adaptable for other OS or network adapter types.

## Background

My motherboard has an Intel network adapter. After installing it's drivers I've seen that it sometimes fails to idenitfty that it's connected to an ethernet cable.

After a lot of online searches, reinstalling the driver multiple times and trying some of the propsed solutions, nothing came of it.

What I've discovered is that restarting the newtork adapter fixes the issue.

## Solution

The solution builds into a Windows service. It can be set up to start up with Windows.

The service checks if the ethernet adapter is unable to transmit data packets. If so, it proceeds to restart the network adapter, and will continue to do so until it can transmit packets.

The solution is easily adaptable for other network adapter types (e.g. Wifi) and other OS types.
