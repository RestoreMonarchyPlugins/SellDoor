# SellDoor
Download latest version from [releases](https://github.com/RestoreMonarchyPlugins/SellDoor/releases)
## About Plugin
This plugin allows players to buy and sell their doors. It's mainly meant to be an open source example of such plugin.

## Features
• Players can sell their doors  
• Players can check the price of the door  
• Players can buy others doors if they are putten on sale  
• Saves the doors that are on sale to `.json` file inside plugin directory

## Commands
**/selldoor <price\>** – Puts on sale the door you look at
``` 
<Permission Cooldown="0">selldoor</Permission>
```
**/costdoor** – Check's for the price of the door look at
``` 
<Permission Cooldown="0">costdoor</Permission>
```
**/buydoor** – Buys the door you look at
``` 
<Permission Cooldown="0">buydoor</Permission>
```

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<SellDoorConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>green</MessageColor>
</SellDoorConfiguration>
```

## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="SellDoorFormat" Value="Use: /selldoor &lt;price&gt;" />
  <Translation Id="SellDoorWrongPrice" Value="{0} is not a right price" />
  <Translation Id="SellDoorSuccess" Value="Successfully put your door on sale for {0}" />
  <Translation Id="SellDoorNotfound" Value="You are not pointing at any door" />
  <Translation Id="SellDoorNotOwner" Value="You are not an owner of that door" />
  <Translation Id="DoorNotFound" Value="You are not pointing at any door" />
  <Translation Id="DoorNotForSale" Value="This door is not on sale" />
  <Translation Id="CostDoorPrice" Value="You can buy this door for {0}" />
  <Translation Id="BuyDoorSuccess" Value="You successfully bought this door for {0}" />
  <Translation Id="BuyDoorCantAfford" Value="You can't afford to buy this door" />
</Translations>
```

