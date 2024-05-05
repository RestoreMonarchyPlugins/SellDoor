# Sell Door
Simple apartment and house plugin for roleplay servers.

## Features
* Players can buy and sell their doors
* Admins have to configure the doors to be sellable
* The doors can be linked to barricades/structures
* The doors data is saved in a `.json` file located in plugin's directory
* The plugin supports UI for buying and selling doors
* You can limit the number of doors a player can own

## Credits
* **Ewad the Sauce God** for sharing his custom UI
* **battletom1233** for sponsoring the addition of the UI to the plugin

## Workshop (optional)
[Sell Door UI](https://steamcommunity.com/sharedfiles/filedetails/?id=3239860033) - `3239860033`

## Commands
* **/selldoor <price\>** – Puts the door on sale
* **/costdoor** – Displays the price of the door
* **/buydoor** – Buys the door you look at
* **/checkdoor** – Displays a door ID, price and owner
* **/deletedoor** – Deletes the door
* **/linkdoor \<id\>** - Links a barricade/structure to specified door id
* **/unlinkdoor** - Unlinks a barricade/structure from the door

## Permissions
```xml
<!-- Player Commands -->
<Permission Cooldown="0">selldoor</Permission>
<Permission Cooldown="0">costdoor</Permission>
<Permission Cooldown="0">buydoor</Permission>
<!-- Admin Commands -->
<Permission Cooldown="0">selldoor.admin</Permission>
<Permission Cooldown="0">checkdoor</Permission>
<Permission Cooldown="0">deletedoor</Permission>
<Permission Cooldown="0">linkdoor</Permission>
<Permission Cooldown="0">unlinkdoor</Permission>
```

## Configuration
```xml
<?xml version="1.0" encoding="utf-8"?>
<SellDoorConfiguration xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <MessageColor>yellow</MessageColor>
  <EnableUI>false</EnableUI>
  <EffectId>54600</EffectId>
  <DefaultMaxDoors>-1</DefaultMaxDoors>
  <Limits>
    <SellDoorLimit>
      <Permission>selldoor.vip</Permission>
      <MaxDoors>3</MaxDoors>
    </SellDoorLimit>
  </Limits>
</SellDoorConfiguration>
```

## Translations
```xml
<?xml version="1.0" encoding="utf-8"?>
<Translations xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
  <Translation Id="DoorNotLooking" Value="You are not looking at any door" />
  <Translation Id="DoorNotForSale" Value="This door is not on sale" />
  <Translation Id="SellDoorFormat" Value="Use: /selldoor &lt;price&gt;" />
  <Translation Id="SellDoorWrongPrice" Value="{0} is not a valid price" />
  <Translation Id="SellDoorSuccess" Value="Successfully put door #{0} on sale for ${1}" />
  <Translation Id="DoorNotOwner" Value="You are not an owner of that door" />
  <Translation Id="CostDoorPrice" Value="You can buy this door for ${0}" />
  <Translation Id="BuyDoorSuccess" Value="You successfully bought this door for ${0}" />
  <Translation Id="BuyDoorCantAfford" Value="You can't afford to buy this door. It costs: ${0}" />
  <Translation Id="BuyDoorLimit" Value="You can't own more than {0} doors!" />
  <Translation Id="DoorAlreadyOnSale" Value="This door is already on sale" />
  <Translation Id="DoorItemNotLooking" Value="You are not looking at any barricade or structure" />
  <Translation Id="LinkDoorFormat" Value="Use: /linkdoor &lt;doorId&gt;" />
  <Translation Id="WrongDoorId" Value="{0} is not a valid doorId" />
  <Translation Id="DoorNotFound" Value="Door with Id {0} was not found" />
  <Translation Id="DoorItemNotOwner" Value="You are not an owner of this {0}" />
  <Translation Id="LinkDoorSuccess" Value="Successfully linked {0} with door #{1}" />
  <Translation Id="CheckDoorSuccess" Value="This is door #{0} for {1} owned by {2}" />
  <Translation Id="DoorItemAlready" Value="This {0} is already linked to door" />
  <Translation Id="NotDoorItem" Value="This {0} is not linked to any door" />
  <Translation Id="UnlinkDoorSuccess" Value="Successfully unlinked {0} with door #{1}" />
  <Translation Id="SellDoorNoPermission" Value="You don't have permission to add a new door on sale" />
  <Translation Id="DeleteDoorSucccess" Value="Successfully deleted door #{0} with {1} items" />
  <Translation Id="SignPropertyOwner" Value="Property owned by {0}" />
  <Translation Id="SignForSale" Value="For sale for ${0}" />
  <Translation Id="SignNotForSale" Value="Not for sale" />
  <Translation Id="UI_Title" Value="Door #{0}" />
  <Translation Id="UI_Owner_Key" Value="Owner" />
  <Translation Id="UI_Price_Key" Value="Price" />
  <Translation Id="UI_BuyButton_Text" Value="Buy Property" />
  <Translation Id="UI_SellButton_Text" Value="Abandon Property" />
  <Translation Id="UI_Owner_Value_You" Value="You" />
  <Translation Id="UI_Owner_Value_Unkown" Value="Dusk Property Group" />
  <Translation Id="UI_Price_Value_NotForSale" Value="Not for sale" />
  <Translation Id="UI_Price_Value" Value="${0}" />
</Translations>
```

