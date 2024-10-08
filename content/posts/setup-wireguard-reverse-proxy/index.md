---
draft: false
date: 2024-02-09T09:40:42+11:00

author: lewis

title: Setup Wireguard Reverse Proxy
summary: This tutorial shows you how you can overcome port forwarding issues through the issue on an encrypted tunnel through WireGuard.

category: Tutorial

tags:
    - networking
    - wireguard
    - tunnel
    - port forwarding
    - reverse proxy

featured: ./static/featured.png
---

## Why do this?

Sometimes you want to host a local server and have it accessible on the wide area network, however sometimes due to
constraints like CGNAT or lack of access to your networking equipment it’s not possible to forward the ports.

Instead we need to setup a secure tunnel to forward packets from a cloud server not behind CGNAT to our local machine.

For this you will need:

- A host behind CGNAT (local machine)
- A mediator server (cloud server)

In this example I am using an Ubuntu 22.04 VPS from BinaryLane (not sponsored, I just like their cheap fast deployment servers) for the mediator server and a local Ubuntu 22.04 Server.

{{< card-warn >}}
Ensure you have a firewall setup on the mediator server to block any connections from IP Addresses other than your own, otherwise anybody will be able to access your local machine through your mediator.
{{< /card-warn >}}

## Setup WireGuard
Before doing anything you need to setup WireGuard on all of the devices involved in the reverse proxy.

### Install WireGuard
#### Ubuntu
Both my mediator and local machine are running Ubuntu Server.

```
sudo apt install wireguard
```

#### Other
For other WireGuard installations you can follow the official guides https://www.wireguard.com/install/

### Setup WireGuard Configs

I use https://www.wireguardconfig.com/ to generate the configs, as writing these by hand is tedious.

However, if you would like to get into the nitty gritty of the configuration you can review the [WireGuard Unofficial Documentation](https://github.com/pirate/wireguard-docs) which is a great resource for WireGuard in general.

#### Note
Don’t use common ports (80, 443, 27015, 25565, etc..) when generating the config as you might run into a port conflict later. I just use the default port `51820`.

### Mediator Server Configuration
Create a file in the home directory called `wg-server.conf` and enter the server config you generated using the WireGuardConfig website.

```
[Interface]
Address = 10.0.0.1/24
ListenPort = 51820
PrivateKey = IN3PnTA6ug7wptdkhqDgGBPkHN0J8WnTpL8+si5ggnI=
PostUp = iptables -A FORWARD -i %i -j ACCEPT; iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE
PostDown = iptables -D FORWARD -i %i -j ACCEPT; iptables -t nat -D POSTROUTING -o eth0 -j MASQUERADE

[Peer]
PublicKey = E7EjICPkwJ2DQ8BYa3I6hX9sbTVriIjjfj4oCAfdHhk=
AllowedIPs = 10.0.0.2/32
```

#### Note
- Do not use the public/private keys used here as these keys are important to ensuring your tunnel is secure and private.
- If you would like to generate your own Public  & Private Key Pair you can [follow this quick-start guide](https://www.wireguard.com/quickstart/#key-generation).

### Mediator Server IP Tables Configuration
The default IP Tables rules that WireGuardConfig generates forwards all traffic to the WireGuard Peer. 
However, It's not a good idea to expose all ports to the user and instead only supply the ports you are going to use.


In this instance I am using port 80 for a web server.
```diff
[Interface]
Address = 10.0.0.1/24
ListenPort = 51820
PrivateKey = IN3PnTA6ug7wptdkhqDgGBPkHN0J8WnTpL8+si5ggnI=
-PostUp = iptables -A FORWARD -i %i -j ACCEPT; iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE
-PostDown = iptables -D FORWARD -i %i -j ACCEPT; iptables -t nat -D POSTROUTING -o eth0 -j MASQUERADE
+PostUp = iptables -A PREROUTING -t nat -i eth0 -p tcp --dport 80 -j DNAT --to-destination 10.0.0.2
+PostUp = iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE
+PostDown = iptables -A PREROUTING -t nat -i eth0 -p tcp --dport 80 -j DNAT --to-destination 10.0.0.2
+PostDown = iptables -t nat -A POSTROUTING -o eth0 -j MASQUERADE

[Peer]
PublicKey = E7EjICPkwJ2DQ8BYa3I6hX9sbTVriIjjfj4oCAfdHhk=
AllowedIPs = 10.0.0.2/32
```
In the above config I have removed the old rules and replaced them with new rules that:
- Rewrites the destination address on traffic entering the `eth0` interface on port `80` to the WireGuard peer.
- Rewrites the source address on the traffic leaving the eth0 interface as the Mediator server.

{{< card-warn >}}
Ensure you have enabled IPv4 Forwarding, otherwise the mediator server will not forward frames to your local machine. For information on how to enable this feature, you can <a href="https://linuxconfig.org/how-to-turn-on-off-ip-forwarding-in-linux">follow this guide.</a>
{{< /card-warn >}}

#### Note
- Replace `10.0.0.2` with your local machine (peer) WireGuard interface IPv4 Address.

### Peer Configuration
Create a file in the home directory called `wg-peer.conf` and enter the peer config you generated using the WireGuardConfig website.

```
[Interface]
Address = 10.0.0.2/24
ListenPort = 51820
PrivateKey = eCJLJ7T+VmaWw5YldkkuF3AV/PFIPl1yLHQPIvHFznc=

[Peer]
PublicKey = 96o6asr29ZwWCGYB/lpqVdE0Ft4K3KWooIiqKQ1/800=
AllowedIPs = 0.0.0.0/0, ::/0
Endpoint = my.mediator-server.net:51820
PersistentKeepalive = 15
```

#### Note
- Do not use the public/private keys used here as these keys are important to ensuring your tunnel is secure and private.
- Replace `my.mediator-server.net` with the IP Address of your mediator server.
- Add the `PersistentKeepalive` option to the Peer section of the config to allow the peer to keep the UDP connection alive. (The value is how many seconds between Keep-alive packets)

## Enable/Disable WireGuard Interface
The wg-quick command allows you to easily bring up or take down a WireGuard interface using the configuration from above.

### Server
```
# Bring the WireGuard interface up.
wg-quick up ~/wg-server.conf

# Bring the WireGuard interface down.
wg-quick down ~/wg-server.conf
```

### Peer
```
# Bring the WireGuard interface up.
wg-quick up ~/wg-peer.conf

# Bring the WireGuard interface down.
wg-quick down ~/wg-peer.conf
```

## Test Ping
You should now be able to ping your server/peer through the WireGuard interface.

```
# Server -> Peer
ping 10.0.0.2

# Peer -> Server
ping 10.0.0.1
```

## Summary
You have now setup WireGuard as a reverse proxy, which allows you to use your intermediary server as a secure tunnel into your local machine.

This is useful if you want to host a server on your local hardware, but you don’t have the option to forward ports.