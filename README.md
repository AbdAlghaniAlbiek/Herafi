# Herafi
<p align="center">
 <img width="300" height="300" src="https://github.com/AbdAlghaniAlbiek/Herafi/blob/master/Herafi.UWP/Assets/logo_400px.png"> 
</p>

![Twitter Follow](https://img.shields.io/twitter/follow/AbdAlbiek?style=social) ![GitHub](https://img.shields.io/github/license/AbdAlghaniAlbiek/SQLiteDBProject)

# Table of content
* [General Info](#general-information)
* [Technologies Used](#technologies-used)
* [Features](#features)
* [Screenshots](#screenshots)
* [Setup](#setup)
* [Versions](#versions)
* [Project Status](#project-status)

* This is my graduation project that I worked on it for 4 months, and It gives `98/100` considered the best graduations projects from
* It's basically A UWP application for admins of Herafi ecosystem. It gives admins the ability to approve the new users and craftmen that they want to working in this ecosystem , also they can see what is going on in whole this organization (like seeing the number of new memebers of users and craftmen weekly or seeing the average of profits monthly and so on) using Analyzes tab.
* It's connected to Node.js backend that also I worked on it, you can see it [here.](https://github.com/AbdAlghaniAlbiek/herafi_backend)

## Technologies Used
* Serialize/Deserialize data.
* Consuming Restfull APIs from the server.
* Animations.
* Glow design.
* High Security level.
* Charts.
* Sign in with Microsoft and Facebook accounts.
* Push notifications.


## Features
* This app can serialize/deserialize the sended/received data from server using `Newtonsoft.Json`.
* Connecting to the server using `Refit` and fetch json data from it using Restfull APIs.
* Using some of animations like scaling anim and ReorderAnimation for GridViews.
* The Glowing design is so beautifull ❤ and it's implemented to all buttons with 3 different colors (blue, red, green) and these colors has Hue's different degrees in dark mode and so that the colors is so matching the UI in dark theme.
* It achieves the high Security level by Implementation this principles:
  1. Encryption/Decryption data that sended/received between server and client using `AES-128-cbc` alghorithm.
  2. Verify the requests that are from signed account not from any user and I achieved this using `JWT` tech.
  3. To verify the token is sended from the right server, I decode token to have `sercret keyword` and check this sercret keyword if it's equal to the `stored secret keyword` in my UWP application or not.
* This application enhanced with `Telerik` charts that help admins to make the right decisions based on its visualization.
* Admins can sign in directly using their Microsoft and Facebook accounts.
* This app can push notifications every 15 minutes even if it was closing using `Windows runtime component`, and it helps admins to now if there is a new members of users or craftmen.
* Supports Light/Dark/System themes.
* Supports two languages Arabic/English.

## Screenshots
> To see all screenshots, you can go [there.](https://github.com/AbdAlghaniAlbiek/Herafi/tree/master/Herafi.UWP/Assets/Screenshots)
<p align="center">
 <img src="https://github.com/AbdAlghaniAlbiek/Herafi/blob/master/Herafi.UWP/Assets/Screenshots/SignIn_Light.jpg"> 
</p>
<p align="center">
 <img src="https://github.com/AbdAlghaniAlbiek/Herafi/blob/master/Herafi.UWP/Assets/Screenshots/Dashboard.png"> 
</p>
<p align="center">
 <img src="https://github.com/AbdAlghaniAlbiek/Herafi/blob/master/Herafi.UWP/Assets/Screenshots/Settings_Light.png"> 
</p>
<p align="center">
 <img src="https://github.com/AbdAlghaniAlbiek/Herafi/blob/master/Herafi.UWP/Assets/Screenshots/Settings_Dark.png"> 
</p>

## Setup
* Visual Studio 2019 at least.
* Windows 10 OS, Version: 1809 update, Build:(10.0, 17763) and above.
* Windows 10, version 1809 SDK.

### Dependencies
* For Herafi.UWP:
  1. CommunityToolkit.Authentication.MSAL.
  2. MaterialLibs.
  3. Microsoft.Toolkit.Uwp.UI.Controls.
  4. Microsoft.Toolkit.Uwp.UI.Animations.
  5. Microsoft.Toolkit.Uwp.UI.media.
  6. Microsoft.Toolkit.Uwp.Connectivity.
  7. Telerik.UI.for.UniversalWindowsPlatform.
  8. winsdkfb.

* For Herafi.Core:
  1. Refit.
  2. Refit.Newtonsoft.Json.
  3. Newtonsoft.Json.

* For Herafi.Background:
  1. Microsoft.Toolkit.Uwp.Notifications.
  
## Versions
**[version 1.0.0]:** It contains on all featurs that are descriped above.

## Project Status
This project no `longer being worked on` but the contributions are still welcome.


