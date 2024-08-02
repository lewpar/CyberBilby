+++
title = 'Spoofing Your MAC'
date = 2024-04-09T18:47:45+10:00
draft = false

summary = 'This tutorial will show you how you can spoof your MAC address.'
featured_image = './content/spoofing-your-mac/featured.png'

categories = ['Tutorial']
tags = ['networking', 'mac-address', 'spoofing', 'hacking']
+++

## What is a MAC address?
A Media Access Control (MAC) address is a 6 byte (48 bits) unique address that is applied to your Network Interface Card (NIC).

This address is made up of two parts which are 3 bytes (24 bits) each, the Vendor specific bytes and the Interface specific bytes.

![MAC Address](/content/spoofing-your-mac/image1.png)

### Vendor Specific Bytes
This portion of the MAC address is assigned to allow you to recognize the vendor/manufacturer the NIC originated from.
![Vendor MAC Portion](/content/spoofing-your-mac/image2.png)

### Interface Specific Bytes
This portion of the MAC address is assigned to ensure each NIC has a Unique Identifier (UID) to differentiate it from other NICs.
![Interface MAC Portion](/content/spoofing-your-mac/image3.png)

## Why would you want to spoof your MAC?
There is a variety of reasons that you may want to spoof your MAC address which includes but is not limited to:
- Hiding the identity of your NIC
- Accessing MAC protected networks
- Man-in-the-Middle Attacks

For the purpose of this tutorial we are focusing on Accessing MAC protected networks.

{{< card-warn >}}
It's not illegal to change or spoof your MAC address, but it is illegal to carry out attacks against networks you do not have permission to penetration test using this technique.
{{< /card-warn >}}

## My Scenario
I recently bought a Nintendo Switch and I wanted to connect this directly via Ethernet due to the weak signal strength of the Wi-Fi NIC inside the Nintendo Switch.

What I decided to do is buy an Ethernet-to-USB Adapter and plug it into my switch, however there was a caveat.
The building I live in provides internet to the residents which has a [Captive Portal](https://en.wikipedia.org/wiki/Captive_portal) on it, which means I have to log in to connect the Nintendo Switch to the internet.

The Nintendo Switch does support captive portals, however the portal being used by our building has some JavaScript that is being blocked by the browser built into the Switch. This means I cannot log in and connect to the network.

### So what do I do?
Captive Portals save the MAC address of the interface that has successfully logged in, so you do not need to log in again after disconnecting and reconnecting to the network.

For the astute readers you may realize where this is going.

If I spoof the MAC address of another device (like my laptop) I can trick the captive portal into thinking my Nintendo Switch has logged into the network and then try to reconnect the switch, bypassing the login.

### How do I spoof the MAC address?
#### Windows
If you are using Windows, you can access your network adapters by:
- Pressing `Windows` + `R` to open the Run utility.
- Typing `cmd` and pressing `Ctrl` + `Shift` + `Enter` to run the Command Prompt with elevated privileges.
- Typing `ncpa.cpl` to access the Network Adapters.

Once you select the network adapter you are currently using:
- Right-click the adapter
- Select `Properties`
- Select `Configure`
- Select the `Advanced` tab
- Look for and select either `Locally Administered Address` or `Network Address`
- Change the selection from `Not Present` to the value above
- Insert the MAC address you gathered from your target device (Nintendo Switch)
- Press OK

{{< card-info >}}
If you are using an Ethernet-to-USB adapter like I am, you need to read the MAC address on that is written physically (usually on a sticker) on the adapter.
{{< /card-info >}}

### Connecting & Re-connecting
I have now spoofed the MAC address of my laptop to be the same as my Nintendo Switch Ethernet-to-USB adapter.

If I connect to the network I will be greeted by the captive portal, and I can now log in to the network.

After logging in the captive portal saved my MAC address for the Nintendo Switch into its database of connected devices.

This means I can now undo the MAC address that I have spoofed in the previous section and reconnect my Nintendo Switch to the network.

## Summary
To sum up what you have learned. Media Access Control (MAC) addresses are Unique Identifiers (UIDs) that are sometimes stored when logging into a Captive Portal. 

You can spoof your Network Interface Card (NIC) MAC address to another NIC MAC address to trick the Captive Portal into thinking the other NIC has logged into the network.