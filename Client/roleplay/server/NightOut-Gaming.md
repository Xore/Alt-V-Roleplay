<a name='assembly'></a>
# NightOut-Gaming

## Contents

- [Accounts](#T-Altv_Roleplay-models-Accounts 'Altv_Roleplay.models.Accounts')
  - [adminLevel](#P-Altv_Roleplay-models-Accounts-adminLevel 'Altv_Roleplay.models.Accounts.adminLevel')
  - [ban](#P-Altv_Roleplay-models-Accounts-ban 'Altv_Roleplay.models.Accounts.ban')
  - [banReason](#P-Altv_Roleplay-models-Accounts-banReason 'Altv_Roleplay.models.Accounts.banReason')
- [Blip](#T-Altv_Roleplay-EntityStreamer-Blip 'Altv_Roleplay.EntityStreamer.Blip')
  - [Color](#P-Altv_Roleplay-EntityStreamer-Blip-Color 'Altv_Roleplay.EntityStreamer.Blip.Color')
  - [Name](#P-Altv_Roleplay-EntityStreamer-Blip-Name 'Altv_Roleplay.EntityStreamer.Blip.Name')
  - [Scale](#P-Altv_Roleplay-EntityStreamer-Blip-Scale 'Altv_Roleplay.EntityStreamer.Blip.Scale')
  - [ShortRange](#P-Altv_Roleplay-EntityStreamer-Blip-ShortRange 'Altv_Roleplay.EntityStreamer.Blip.ShortRange')
  - [Sprite](#P-Altv_Roleplay-EntityStreamer-Blip-Sprite 'Altv_Roleplay.EntityStreamer.Blip.Sprite')
  - [Delete()](#M-Altv_Roleplay-EntityStreamer-Blip-Delete 'Altv_Roleplay.EntityStreamer.Blip.Delete')
- [BlipStreamer](#T-Altv_Roleplay-EntityStreamer-BlipStreamer 'Altv_Roleplay.EntityStreamer.BlipStreamer')
  - [CreateDynamicBlip(name,color,scale,shortRange,spriteId,position,dimension,range)](#M-Altv_Roleplay-EntityStreamer-BlipStreamer-CreateDynamicBlip-System-String,System-Int32,System-Single,System-Boolean,System-Int32,System-Numerics-Vector3,System-Int32,System-UInt32- 'Altv_Roleplay.EntityStreamer.BlipStreamer.CreateDynamicBlip(System.String,System.Int32,System.Single,System.Boolean,System.Int32,System.Numerics.Vector3,System.Int32,System.UInt32)')
  - [CreateStaticBlip(name,color,scale,shortRange,spriteId,position,dimension,range)](#M-Altv_Roleplay-EntityStreamer-BlipStreamer-CreateStaticBlip-System-String,System-Int32,System-Single,System-Boolean,System-Int32,System-Numerics-Vector3,System-Int32,System-UInt32- 'Altv_Roleplay.EntityStreamer.BlipStreamer.CreateStaticBlip(System.String,System.Int32,System.Single,System.Boolean,System.Int32,System.Numerics.Vector3,System.Int32,System.UInt32)')
  - [DestroyBlip(blip)](#M-Altv_Roleplay-EntityStreamer-BlipStreamer-DestroyBlip-Altv_Roleplay-EntityStreamer-Blip- 'Altv_Roleplay.EntityStreamer.BlipStreamer.DestroyBlip(Altv_Roleplay.EntityStreamer.Blip)')
- [Characters](#T-Altv_Roleplay-Model-Characters 'Altv_Roleplay.Model.Characters')
  - [SetCharacterFastFarm(charId,isFastFarm,fastFarmTime)](#M-Altv_Roleplay-Model-Characters-SetCharacterFastFarm-System-Int32,System-Boolean,System-Int32- 'Altv_Roleplay.Model.Characters.SetCharacterFastFarm(System.Int32,System.Boolean,System.Int32)')
- [DropShadow](#T-Altv_Roleplay-EntityStreamer-DropShadow 'Altv_Roleplay.EntityStreamer.DropShadow')
- [HelpText](#T-Altv_Roleplay-EntityStreamer-HelpText 'Altv_Roleplay.EntityStreamer.HelpText')
  - [Text](#P-Altv_Roleplay-EntityStreamer-HelpText-Text 'Altv_Roleplay.EntityStreamer.HelpText.Text')
  - [Delete()](#M-Altv_Roleplay-EntityStreamer-HelpText-Delete 'Altv_Roleplay.EntityStreamer.HelpText.Delete')
- [HelpTextStreamer](#T-Altv_Roleplay-EntityStreamer-HelpTextStreamer 'Altv_Roleplay.EntityStreamer.HelpTextStreamer')
  - [Create(text,dimension,streamRange,position)](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-Create-System-String,System-Numerics-Vector3,System-Int32,System-UInt32- 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.Create(System.String,System.Numerics.Vector3,System.Int32,System.UInt32)')
  - [DeleteAllHelpText()](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteAllHelpText 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.DeleteAllHelpText')
  - [DeleteHelpText(dynamicTextLabelId)](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteHelpText-System-UInt64- 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.DeleteHelpText(System.UInt64)')
  - [DeleteHelpText(dynamicTextLabel)](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteHelpText-Altv_Roleplay-EntityStreamer-HelpText- 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.DeleteHelpText(Altv_Roleplay.EntityStreamer.HelpText)')
  - [GetAllHelpText()](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-GetAllHelpText 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.GetAllHelpText')
  - [GetHelpText(dynamicTextLabelId)](#M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-GetHelpText-System-UInt64- 'Altv_Roleplay.EntityStreamer.HelpTextStreamer.GetHelpText(System.UInt64)')
- [Marker](#T-Altv_Roleplay-EntityStreamer-Marker 'Altv_Roleplay.EntityStreamer.Marker')
  - [BobUpDown](#P-Altv_Roleplay-EntityStreamer-Marker-BobUpDown 'Altv_Roleplay.EntityStreamer.Marker.BobUpDown')
  - [Color](#P-Altv_Roleplay-EntityStreamer-Marker-Color 'Altv_Roleplay.EntityStreamer.Marker.Color')
  - [Direction](#P-Altv_Roleplay-EntityStreamer-Marker-Direction 'Altv_Roleplay.EntityStreamer.Marker.Direction')
  - [DrawOnEnter](#P-Altv_Roleplay-EntityStreamer-Marker-DrawOnEnter 'Altv_Roleplay.EntityStreamer.Marker.DrawOnEnter')
  - [FaceCamera](#P-Altv_Roleplay-EntityStreamer-Marker-FaceCamera 'Altv_Roleplay.EntityStreamer.Marker.FaceCamera')
  - [MarkerType](#P-Altv_Roleplay-EntityStreamer-Marker-MarkerType 'Altv_Roleplay.EntityStreamer.Marker.MarkerType')
  - [Rotate](#P-Altv_Roleplay-EntityStreamer-Marker-Rotate 'Altv_Roleplay.EntityStreamer.Marker.Rotate')
  - [Rotation](#P-Altv_Roleplay-EntityStreamer-Marker-Rotation 'Altv_Roleplay.EntityStreamer.Marker.Rotation')
  - [Scale](#P-Altv_Roleplay-EntityStreamer-Marker-Scale 'Altv_Roleplay.EntityStreamer.Marker.Scale')
  - [TextureDict](#P-Altv_Roleplay-EntityStreamer-Marker-TextureDict 'Altv_Roleplay.EntityStreamer.Marker.TextureDict')
  - [TextureName](#P-Altv_Roleplay-EntityStreamer-Marker-TextureName 'Altv_Roleplay.EntityStreamer.Marker.TextureName')
  - [Destroy()](#M-Altv_Roleplay-EntityStreamer-Marker-Destroy 'Altv_Roleplay.EntityStreamer.Marker.Destroy')
- [MarkerStreamer](#T-Altv_Roleplay-EntityStreamer-MarkerStreamer 'Altv_Roleplay.EntityStreamer.MarkerStreamer')
  - [Create(markerType,position,scale,rotation,direction,color,bobUpDown,faceCamera,rotate,textureDict,textureName,drawOnEnter,dimension,streamRange)](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Create-Altv_Roleplay-EntityStreamer-MarkerTypes,System-Numerics-Vector3,System-Numerics-Vector3,System-Nullable{System-Numerics-Vector3},System-Nullable{System-Numerics-Vector3},System-Nullable{AltV-Net-Data-Rgba},System-Int32,System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-String,System-String,System-Nullable{System-Boolean},System-UInt32- 'Altv_Roleplay.EntityStreamer.MarkerStreamer.Create(Altv_Roleplay.EntityStreamer.MarkerTypes,System.Numerics.Vector3,System.Numerics.Vector3,System.Nullable{System.Numerics.Vector3},System.Nullable{System.Numerics.Vector3},System.Nullable{AltV.Net.Data.Rgba},System.Int32,System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.String,System.String,System.Nullable{System.Boolean},System.UInt32)')
  - [Delete(dynamicMarkerId)](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Delete-System-UInt64- 'Altv_Roleplay.EntityStreamer.MarkerStreamer.Delete(System.UInt64)')
  - [Delete(marker)](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Delete-Altv_Roleplay-EntityStreamer-Marker- 'Altv_Roleplay.EntityStreamer.MarkerStreamer.Delete(Altv_Roleplay.EntityStreamer.Marker)')
  - [DestroyAllDynamicMarkers()](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-DestroyAllDynamicMarkers 'Altv_Roleplay.EntityStreamer.MarkerStreamer.DestroyAllDynamicMarkers')
  - [GetAllDynamicMarkers()](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-GetAllDynamicMarkers 'Altv_Roleplay.EntityStreamer.MarkerStreamer.GetAllDynamicMarkers')
  - [GetMarker()](#M-Altv_Roleplay-EntityStreamer-MarkerStreamer-GetMarker-System-UInt64- 'Altv_Roleplay.EntityStreamer.MarkerStreamer.GetMarker(System.UInt64)')
- [MarkerTypes](#T-Altv_Roleplay-EntityStreamer-MarkerTypes 'Altv_Roleplay.EntityStreamer.MarkerTypes')
- [Ped](#T-Altv_Roleplay-EntityStreamer-Ped 'Altv_Roleplay.EntityStreamer.Ped')
  - [Model](#P-Altv_Roleplay-EntityStreamer-Ped-Model 'Altv_Roleplay.EntityStreamer.Ped.Model')
- [PedStreamer](#T-Altv_Roleplay-EntityStreamer-PedStreamer 'Altv_Roleplay.EntityStreamer.PedStreamer')
  - [Create()](#M-Altv_Roleplay-EntityStreamer-PedStreamer-Create-System-String,System-Numerics-Vector3,System-Numerics-Vector3,System-Int32,System-UInt32- 'Altv_Roleplay.EntityStreamer.PedStreamer.Create(System.String,System.Numerics.Vector3,System.Numerics.Vector3,System.Int32,System.UInt32)')
- [PlayerLabel](#T-Altv_Roleplay-EntityStreamer-PlayerLabel 'Altv_Roleplay.EntityStreamer.PlayerLabel')
  - [Center](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Center 'Altv_Roleplay.EntityStreamer.PlayerLabel.Center')
  - [Color](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Color 'Altv_Roleplay.EntityStreamer.PlayerLabel.Color')
  - [DropShadow](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-DropShadow 'Altv_Roleplay.EntityStreamer.PlayerLabel.DropShadow')
  - [Edge](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Edge 'Altv_Roleplay.EntityStreamer.PlayerLabel.Edge')
  - [Font](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Font 'Altv_Roleplay.EntityStreamer.PlayerLabel.Font')
  - [Proportional](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Proportional 'Altv_Roleplay.EntityStreamer.PlayerLabel.Proportional')
  - [Scale](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Scale 'Altv_Roleplay.EntityStreamer.PlayerLabel.Scale')
  - [Text](#P-Altv_Roleplay-EntityStreamer-PlayerLabel-Text 'Altv_Roleplay.EntityStreamer.PlayerLabel.Text')
  - [Delete()](#M-Altv_Roleplay-EntityStreamer-PlayerLabel-Delete 'Altv_Roleplay.EntityStreamer.PlayerLabel.Delete')
- [Prop](#T-Altv_Roleplay-EntityStreamer-Prop 'Altv_Roleplay.EntityStreamer.Prop')
  - [Dynamic](#P-Altv_Roleplay-EntityStreamer-Prop-Dynamic 'Altv_Roleplay.EntityStreamer.Prop.Dynamic')
  - [Freeze](#P-Altv_Roleplay-EntityStreamer-Prop-Freeze 'Altv_Roleplay.EntityStreamer.Prop.Freeze')
  - [LightColor](#P-Altv_Roleplay-EntityStreamer-Prop-LightColor 'Altv_Roleplay.EntityStreamer.Prop.LightColor')
  - [LodDistance](#P-Altv_Roleplay-EntityStreamer-Prop-LodDistance 'Altv_Roleplay.EntityStreamer.Prop.LodDistance')
  - [Model](#P-Altv_Roleplay-EntityStreamer-Prop-Model 'Altv_Roleplay.EntityStreamer.Prop.Model')
  - [OnFire](#P-Altv_Roleplay-EntityStreamer-Prop-OnFire 'Altv_Roleplay.EntityStreamer.Prop.OnFire')
  - [Rotation](#P-Altv_Roleplay-EntityStreamer-Prop-Rotation 'Altv_Roleplay.EntityStreamer.Prop.Rotation')
  - [TextureVariation](#P-Altv_Roleplay-EntityStreamer-Prop-TextureVariation 'Altv_Roleplay.EntityStreamer.Prop.TextureVariation')
  - [Visible](#P-Altv_Roleplay-EntityStreamer-Prop-Visible 'Altv_Roleplay.EntityStreamer.Prop.Visible')
- [PropStreamer](#T-Altv_Roleplay-EntityStreamer-PropStreamer 'Altv_Roleplay.EntityStreamer.PropStreamer')
  - [Create(model,position,rotation,dimension,isDynamic,placeObjectOnGroundProperly,frozen,lodDistance,lightColor,onFire,textureVariation,visible,streamRange)](#M-Altv_Roleplay-EntityStreamer-PropStreamer-Create-System-String,System-Numerics-Vector3,System-Numerics-Vector3,System-Int32,System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-UInt32},Altv_Roleplay-EntityStreamer-Rgb,System-Nullable{System-Boolean},System-Nullable{Altv_Roleplay-EntityStreamer-TextureVariation},System-Nullable{System-Boolean},System-UInt32- 'Altv_Roleplay.EntityStreamer.PropStreamer.Create(System.String,System.Numerics.Vector3,System.Numerics.Vector3,System.Int32,System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.Boolean},System.Nullable{System.UInt32},Altv_Roleplay.EntityStreamer.Rgb,System.Nullable{System.Boolean},System.Nullable{Altv_Roleplay.EntityStreamer.TextureVariation},System.Nullable{System.Boolean},System.UInt32)')
  - [DestroyAllDynamicObjects()](#M-Altv_Roleplay-EntityStreamer-PropStreamer-DestroyAllDynamicObjects 'Altv_Roleplay.EntityStreamer.PropStreamer.DestroyAllDynamicObjects')
  - [GetAllProp()](#M-Altv_Roleplay-EntityStreamer-PropStreamer-GetAllProp 'Altv_Roleplay.EntityStreamer.PropStreamer.GetAllProp')
  - [GetClosestDynamicObject(pos)](#M-Altv_Roleplay-EntityStreamer-PropStreamer-GetClosestDynamicObject-System-Numerics-Vector3- 'Altv_Roleplay.EntityStreamer.PropStreamer.GetClosestDynamicObject(System.Numerics.Vector3)')
- [TextLabelStreamer](#T-Altv_Roleplay-EntityStreamer-TextLabelStreamer 'Altv_Roleplay.EntityStreamer.TextLabelStreamer')
  - [Create(text,position,dimension,center,color,scale,dropShadow,edge,font,proportional,streamRange)](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-Create-System-String,System-Numerics-Vector3,System-Int32,System-Nullable{System-Boolean},System-Nullable{AltV-Net-Data-Rgba},System-Nullable{System-Single},Altv_Roleplay-EntityStreamer-DropShadow,System-Nullable{AltV-Net-Data-Rgba},System-Nullable{System-Int32},System-Nullable{System-Boolean},System-UInt32- 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.Create(System.String,System.Numerics.Vector3,System.Int32,System.Nullable{System.Boolean},System.Nullable{AltV.Net.Data.Rgba},System.Nullable{System.Single},Altv_Roleplay.EntityStreamer.DropShadow,System.Nullable{AltV.Net.Data.Rgba},System.Nullable{System.Int32},System.Nullable{System.Boolean},System.UInt32)')
  - [DestroyAllDynamicTextLabels()](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyAllDynamicTextLabels 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.DestroyAllDynamicTextLabels')
  - [DestroyDynamicTextLabel(dynamicTextLabelId)](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyDynamicTextLabel-System-UInt64- 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.DestroyDynamicTextLabel(System.UInt64)')
  - [DestroyDynamicTextLabel(dynamicTextLabel)](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyDynamicTextLabel-Altv_Roleplay-EntityStreamer-PlayerLabel- 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.DestroyDynamicTextLabel(Altv_Roleplay.EntityStreamer.PlayerLabel)')
  - [GetAllDynamicTextLabels()](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-GetAllDynamicTextLabels 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.GetAllDynamicTextLabels')
  - [GetDynamicTextLabel(dynamicTextLabelId)](#M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-GetDynamicTextLabel-System-UInt64- 'Altv_Roleplay.EntityStreamer.TextLabelStreamer.GetDynamicTextLabel(System.UInt64)')
- [VirtualAPI](#T-Global-mGlobal-VirtualAPI 'Global.mGlobal.VirtualAPI')
  - [RunThreadSafe(function)](#M-Global-mGlobal-VirtualAPI-RunThreadSafe-System-Action- 'Global.mGlobal.VirtualAPI.RunThreadSafe(System.Action)')

<a name='T-Altv_Roleplay-models-Accounts'></a>
## Accounts `type`

##### Namespace

Altv_Roleplay.models

##### Summary



<a name='P-Altv_Roleplay-models-Accounts-adminLevel'></a>
### adminLevel `property`

##### Summary



<a name='P-Altv_Roleplay-models-Accounts-ban'></a>
### ban `property`

##### Summary



<a name='P-Altv_Roleplay-models-Accounts-banReason'></a>
### banReason `property`

##### Summary



<a name='T-Altv_Roleplay-EntityStreamer-Blip'></a>
## Blip `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

Blip class that stores all data related to a single blip.

<a name='P-Altv_Roleplay-EntityStreamer-Blip-Color'></a>
### Color `property`

##### Summary

Blip Color code, can also be found on the ALTV wiki

<a name='P-Altv_Roleplay-EntityStreamer-Blip-Name'></a>
### Name `property`

##### Summary

The text to display on the blip in the map menu

<a name='P-Altv_Roleplay-EntityStreamer-Blip-Scale'></a>
### Scale `property`

##### Summary

Scale of the blip, 1 is regular size.

<a name='P-Altv_Roleplay-EntityStreamer-Blip-ShortRange'></a>
### ShortRange `property`

##### Summary

Whether this blip can be seen on the minimap from anywhere on the map, or only when close to it(it will always show on the main map).

<a name='P-Altv_Roleplay-EntityStreamer-Blip-Sprite'></a>
### Sprite `property`

##### Summary

ID of the sprite to use, can be found on the ALTV wiki

<a name='M-Altv_Roleplay-EntityStreamer-Blip-Delete'></a>
### Delete() `method`

##### Summary

Destroy this blip.

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-BlipStreamer'></a>
## BlipStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-BlipStreamer-CreateDynamicBlip-System-String,System-Int32,System-Single,System-Boolean,System-Int32,System-Numerics-Vector3,System-Int32,System-UInt32-'></a>
### CreateDynamicBlip(name,color,scale,shortRange,spriteId,position,dimension,range) `method`

##### Summary

Create Dynamic Blip.

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| color | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| scale | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') |  |
| shortRange | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |
| spriteId | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| position | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') |  |
| dimension | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| range | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') |  |

<a name='M-Altv_Roleplay-EntityStreamer-BlipStreamer-CreateStaticBlip-System-String,System-Int32,System-Single,System-Boolean,System-Int32,System-Numerics-Vector3,System-Int32,System-UInt32-'></a>
### CreateStaticBlip(name,color,scale,shortRange,spriteId,position,dimension,range) `method`

##### Summary

Create static blip without any range limit

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| name | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') |  |
| color | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| scale | [System.Single](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Single 'System.Single') |  |
| shortRange | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |
| spriteId | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| position | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') |  |
| dimension | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| range | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') |  |

<a name='M-Altv_Roleplay-EntityStreamer-BlipStreamer-DestroyBlip-Altv_Roleplay-EntityStreamer-Blip-'></a>
### DestroyBlip(blip) `method`

##### Summary

Destroy a dynamic blip

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| blip | [Altv_Roleplay.EntityStreamer.Blip](#T-Altv_Roleplay-EntityStreamer-Blip 'Altv_Roleplay.EntityStreamer.Blip') | The blip to destroy |

<a name='T-Altv_Roleplay-Model-Characters'></a>
## Characters `type`

##### Namespace

Altv_Roleplay.Model

<a name='M-Altv_Roleplay-Model-Characters-SetCharacterFastFarm-System-Int32,System-Boolean,System-Int32-'></a>
### SetCharacterFastFarm(charId,isFastFarm,fastFarmTime) `method`

##### Summary

Setzt Character für eine bestimmbare Zeit auf doppelte Aufsammelmenge

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| charId | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| isFastFarm | [System.Boolean](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Boolean 'System.Boolean') |  |
| fastFarmTime | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |

<a name='T-Altv_Roleplay-EntityStreamer-DropShadow'></a>
## DropShadow `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

Class to hold drop shadow data.

<a name='T-Altv_Roleplay-EntityStreamer-HelpText'></a>
## HelpText `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

HelpText class that stores all data

<a name='P-Altv_Roleplay-EntityStreamer-HelpText-Text'></a>
### Text `property`

##### Summary

Set/get the HelpText text.

<a name='M-Altv_Roleplay-EntityStreamer-HelpText-Delete'></a>
### Delete() `method`

##### Summary

Destroy this textlabel.

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-HelpTextStreamer'></a>
## HelpTextStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-Create-System-String,System-Numerics-Vector3,System-Int32,System-UInt32-'></a>
### Create(text,dimension,streamRange,position) `method`

##### Summary

Create a new HelpText.

##### Returns

The newly created dynamic textlabel.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text to be displayed. |
| dimension | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') |  |
| streamRange | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') |  |
| position | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') |  |

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteAllHelpText'></a>
### DeleteAllHelpText() `method`

##### Summary

Destroy all HelpText.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteHelpText-System-UInt64-'></a>
### DeleteHelpText(dynamicTextLabelId) `method`

##### Summary

Destroy HelpText by it's ID.

##### Returns

True if successful, false otherwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabelId | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | The ID of the text label. |

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-DeleteHelpText-Altv_Roleplay-EntityStreamer-HelpText-'></a>
### DeleteHelpText(dynamicTextLabel) `method`

##### Summary

Destroy an HelpText.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabel | [Altv_Roleplay.EntityStreamer.HelpText](#T-Altv_Roleplay-EntityStreamer-HelpText 'Altv_Roleplay.EntityStreamer.HelpText') | The text label instance to destroy. |

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-GetAllHelpText'></a>
### GetAllHelpText() `method`

##### Summary

Get all HelpText.

##### Returns

A list of dynamic textlabels.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-HelpTextStreamer-GetHelpText-System-UInt64-'></a>
### GetHelpText(dynamicTextLabelId) `method`

##### Summary

Get a HelpText by it's ID.

##### Returns

The dynamic textlabel or null if not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabelId | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | The ID of the textlabel. |

<a name='T-Altv_Roleplay-EntityStreamer-Marker'></a>
## Marker `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

DynamicMarker class that stores all data related to a single marker.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-BobUpDown'></a>
### BobUpDown `property`

##### Summary

Whether the marker should bob up and down.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-Color'></a>
### Color `property`

##### Summary

Set marker color.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-Direction'></a>
### Direction `property`

##### Summary

Represents a heading on each axis in which the marker should face, alternatively you can rotate each axis independently with Rotation and set Direction axis to 0.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-DrawOnEnter'></a>
### DrawOnEnter `property`

##### Summary

Whether the marker should be drawn onto the entity when they enter it.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-FaceCamera'></a>
### FaceCamera `property`

##### Summary

Whether the marker should rotate on the Y axis towards the player's camera.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-MarkerType'></a>
### MarkerType `property`

##### Summary

Set or get the current marker's type(see MarkerTypes enum).

<a name='P-Altv_Roleplay-EntityStreamer-Marker-Rotate'></a>
### Rotate `property`

##### Summary

Whether the marker should rotate on the Y axis(heading).

<a name='P-Altv_Roleplay-EntityStreamer-Marker-Rotation'></a>
### Rotation `property`

##### Summary

Set or get the current marker's rotation (in degrees).

<a name='P-Altv_Roleplay-EntityStreamer-Marker-Scale'></a>
### Scale `property`

##### Summary

Set scale of the marker.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-TextureDict'></a>
### TextureDict `property`

##### Summary

Set a texture dictionary, pass null to remove.

<a name='P-Altv_Roleplay-EntityStreamer-Marker-TextureName'></a>
### TextureName `property`

##### Summary

Texture name, only applicable if TextureDict is set. pass null to reset value.

<a name='M-Altv_Roleplay-EntityStreamer-Marker-Destroy'></a>
### Destroy() `method`

##### Summary

Destroy this marker.

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-MarkerStreamer'></a>
## MarkerStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Create-Altv_Roleplay-EntityStreamer-MarkerTypes,System-Numerics-Vector3,System-Numerics-Vector3,System-Nullable{System-Numerics-Vector3},System-Nullable{System-Numerics-Vector3},System-Nullable{AltV-Net-Data-Rgba},System-Int32,System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-String,System-String,System-Nullable{System-Boolean},System-UInt32-'></a>
### Create(markerType,position,scale,rotation,direction,color,bobUpDown,faceCamera,rotate,textureDict,textureName,drawOnEnter,dimension,streamRange) `method`

##### Summary

Create a new dynamic marker

##### Returns



##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| markerType | [Altv_Roleplay.EntityStreamer.MarkerTypes](#T-Altv_Roleplay-EntityStreamer-MarkerTypes 'Altv_Roleplay.EntityStreamer.MarkerTypes') | The type of marker to spawn. |
| position | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The position at which the marker should spawn at. |
| scale | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The scale of the marker. |
| rotation | [System.Nullable{System.Numerics.Vector3}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Numerics.Vector3}') | The rotation of the marker. |
| direction | [System.Nullable{System.Numerics.Vector3}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Numerics.Vector3}') | The direction of the marker. |
| color | [System.Nullable{AltV.Net.Data.Rgba}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{AltV.Net.Data.Rgba}') | The color of the marker. |
| bobUpDown | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | Whether the marker should bob up and down. |
| faceCamera | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | Whether the marker should face the entity's camera. |
| rotate | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | Whether the marker should rotate on the Y axis only. |
| textureDict | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | An optional texture dictionary to apply to the marker. |
| textureName | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | An optional texture name to apply to the marker. |
| drawOnEnter | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | Whether it should draw the marker onto an entity that intersects with it. |
| dimension | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | The dimension of the marker |
| streamRange | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') | Stream distance of the marker, default is 100. |

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Delete-System-UInt64-'></a>
### Delete(dynamicMarkerId) `method`

##### Summary

Destroy a dynamic marker by it's ID.

##### Returns

True if successful, false otherwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicMarkerId | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | The ID of the marker. |

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-Delete-Altv_Roleplay-EntityStreamer-Marker-'></a>
### Delete(marker) `method`

##### Summary

Destroy a dynamic marker.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| marker | [Altv_Roleplay.EntityStreamer.Marker](#T-Altv_Roleplay-EntityStreamer-Marker 'Altv_Roleplay.EntityStreamer.Marker') | The marker instance to destroy. |

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-DestroyAllDynamicMarkers'></a>
### DestroyAllDynamicMarkers() `method`

##### Summary

Destroy all created dynamic markers.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-GetAllDynamicMarkers'></a>
### GetAllDynamicMarkers() `method`

##### Summary

Get all created dynamic markers.

##### Returns

A list of dynamic markers.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-MarkerStreamer-GetMarker-System-UInt64-'></a>
### GetMarker() `method`

##### Summary

Get a dynamic marker by it's ID.

##### Returns

The dynamic marker or null if not found.

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-MarkerTypes'></a>
## MarkerTypes `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

Marker types.

<a name='T-Altv_Roleplay-EntityStreamer-Ped'></a>
## Ped `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='P-Altv_Roleplay-EntityStreamer-Ped-Model'></a>
### Model `property`

##### Summary

Set or get the current ped model.

<a name='T-Altv_Roleplay-EntityStreamer-PedStreamer'></a>
## PedStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-PedStreamer-Create-System-String,System-Numerics-Vector3,System-Numerics-Vector3,System-Int32,System-UInt32-'></a>
### Create() `method`

##### Summary

Create Ped

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-PlayerLabel'></a>
## PlayerLabel `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

DynamicTextLabel class that stores all data related to a single textlabel

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Center'></a>
### Center `property`

##### Summary

Set/get textlabel center, if true the textlabel will be centered.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Color'></a>
### Color `property`

##### Summary

Set/get textlabel's color.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-DropShadow'></a>
### DropShadow `property`

##### Summary

Set/get textlabel's drop shadow.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Edge'></a>
### Edge `property`

##### Summary

Set/get textlabel's edge color.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Font'></a>
### Font `property`

##### Summary

Set/get textlabel's font type.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Proportional'></a>
### Proportional `property`

##### Summary

Set/get textlabel proportional.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Scale'></a>
### Scale `property`

##### Summary

Set/get or get the current textlabel's scale.

<a name='P-Altv_Roleplay-EntityStreamer-PlayerLabel-Text'></a>
### Text `property`

##### Summary

Set/get the textlabel's text.

<a name='M-Altv_Roleplay-EntityStreamer-PlayerLabel-Delete'></a>
### Delete() `method`

##### Summary

Destroy this textlabel.

##### Parameters

This method has no parameters.

<a name='T-Altv_Roleplay-EntityStreamer-Prop'></a>
## Prop `type`

##### Namespace

Altv_Roleplay.EntityStreamer

##### Summary

DynamicObject class that stores all data related to a single object

<a name='P-Altv_Roleplay-EntityStreamer-Prop-Dynamic'></a>
### Dynamic `property`

##### Summary

Get or set the object's dynamic state. Some objects can be moved around by the player when dynamic is set to true.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-Freeze'></a>
### Freeze `property`

##### Summary

Freeze an object into it's current position. or get it's status

<a name='P-Altv_Roleplay-EntityStreamer-Prop-LightColor'></a>
### LightColor `property`

##### Summary

Set the light color of the object, use null to reset it to default.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-LodDistance'></a>
### LodDistance `property`

##### Summary

Set or get LOD Distance of the object.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-Model'></a>
### Model `property`

##### Summary

Set or get the current object's model.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-OnFire'></a>
### OnFire `property`

##### Summary

Set/get an object on fire, NOTE: does not work very well as of right now, fire is very small.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-Rotation'></a>
### Rotation `property`

##### Summary

Set or get the current object's rotation (in degrees).

<a name='P-Altv_Roleplay-EntityStreamer-Prop-TextureVariation'></a>
### TextureVariation `property`

##### Summary

Get or set the current texture variation, use null to reset it to default.

<a name='P-Altv_Roleplay-EntityStreamer-Prop-Visible'></a>
### Visible `property`

##### Summary

Set/get visibility state of object

<a name='T-Altv_Roleplay-EntityStreamer-PropStreamer'></a>
## PropStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-PropStreamer-Create-System-String,System-Numerics-Vector3,System-Numerics-Vector3,System-Int32,System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-Boolean},System-Nullable{System-UInt32},Altv_Roleplay-EntityStreamer-Rgb,System-Nullable{System-Boolean},System-Nullable{Altv_Roleplay-EntityStreamer-TextureVariation},System-Nullable{System-Boolean},System-UInt32-'></a>
### Create(model,position,rotation,dimension,isDynamic,placeObjectOnGroundProperly,frozen,lodDistance,lightColor,onFire,textureVariation,visible,streamRange) `method`

##### Summary

Create a new dynamic object.

##### Returns

The newly created dynamic object.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| model | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The object model name. |
| position | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The position to spawn the object at. |
| rotation | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The rotation to spawn the object at(degrees). |
| dimension | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The dimension to spawn the object in. |
| isDynamic | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | (Optional): Set object dynamic or not. |
| placeObjectOnGroundProperly | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') |  |
| frozen | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | (Optional): Set object frozen. |
| lodDistance | [System.Nullable{System.UInt32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.UInt32}') | (Optional): Set LOD distance. |
| lightColor | [Altv_Roleplay.EntityStreamer.Rgb](#T-Altv_Roleplay-EntityStreamer-Rgb 'Altv_Roleplay.EntityStreamer.Rgb') | (Optional): set light color. |
| onFire | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | (Optional): set object on fire(DOESN'T WORK PROPERLY YET!) |
| textureVariation | [System.Nullable{Altv_Roleplay.EntityStreamer.TextureVariation}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{Altv_Roleplay.EntityStreamer.TextureVariation}') | (Optional): Set object texture variation. |
| visible | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | (Optional): Set object visibility. |
| streamRange | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') | (Optional): The range that a player has to be in before the object spawns, default value is 400. |

<a name='M-Altv_Roleplay-EntityStreamer-PropStreamer-DestroyAllDynamicObjects'></a>
### DestroyAllDynamicObjects() `method`

##### Summary

Destroy all created dynamic objects.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-PropStreamer-GetAllProp'></a>
### GetAllProp() `method`

##### Summary

Get all created dynamic objects.

##### Returns

A list of dynamic objects.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-PropStreamer-GetClosestDynamicObject-System-Numerics-Vector3-'></a>
### GetClosestDynamicObject(pos) `method`

##### Summary

Get the dynamic object that's closest to a specified position.

##### Returns

The closest dynamic object to the specified position, or null if none found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| pos | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The position from which to check. |

<a name='T-Altv_Roleplay-EntityStreamer-TextLabelStreamer'></a>
## TextLabelStreamer `type`

##### Namespace

Altv_Roleplay.EntityStreamer

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-Create-System-String,System-Numerics-Vector3,System-Int32,System-Nullable{System-Boolean},System-Nullable{AltV-Net-Data-Rgba},System-Nullable{System-Single},Altv_Roleplay-EntityStreamer-DropShadow,System-Nullable{AltV-Net-Data-Rgba},System-Nullable{System-Int32},System-Nullable{System-Boolean},System-UInt32-'></a>
### Create(text,position,dimension,center,color,scale,dropShadow,edge,font,proportional,streamRange) `method`

##### Summary

Create a new dynamic textlabel.

##### Returns

The newly created dynamic textlabel.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| text | [System.String](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.String 'System.String') | The text to be displayed. |
| position | [System.Numerics.Vector3](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Numerics.Vector3 'System.Numerics.Vector3') | The position to spawn it at. |
| dimension | [System.Int32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Int32 'System.Int32') | The dimension to spawn it in. |
| center | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | Center the textlabel. |
| color | [System.Nullable{AltV.Net.Data.Rgba}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{AltV.Net.Data.Rgba}') | The color of the textlabel. |
| scale | [System.Nullable{System.Single}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Single}') | The scale of the textlabel. |
| dropShadow | [Altv_Roleplay.EntityStreamer.DropShadow](#T-Altv_Roleplay-EntityStreamer-DropShadow 'Altv_Roleplay.EntityStreamer.DropShadow') | The drop shadow of the textlabel. |
| edge | [System.Nullable{AltV.Net.Data.Rgba}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{AltV.Net.Data.Rgba}') | The edge color of the textlabel. |
| font | [System.Nullable{System.Int32}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Int32}') | The font type of the textlabel. |
| proportional | [System.Nullable{System.Boolean}](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Nullable 'System.Nullable{System.Boolean}') | Whether to set textlabel proportional. |
| streamRange | [System.UInt32](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt32 'System.UInt32') | Stream range, default is 30. |

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyAllDynamicTextLabels'></a>
### DestroyAllDynamicTextLabels() `method`

##### Summary

Destroy all created dynamic textlabels.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyDynamicTextLabel-System-UInt64-'></a>
### DestroyDynamicTextLabel(dynamicTextLabelId) `method`

##### Summary

Destroy a dynamic text label by it's ID.

##### Returns

True if successful, false otherwise.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabelId | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | The ID of the text label. |

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-DestroyDynamicTextLabel-Altv_Roleplay-EntityStreamer-PlayerLabel-'></a>
### DestroyDynamicTextLabel(dynamicTextLabel) `method`

##### Summary

Destroy a dynamic text label.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabel | [Altv_Roleplay.EntityStreamer.PlayerLabel](#T-Altv_Roleplay-EntityStreamer-PlayerLabel 'Altv_Roleplay.EntityStreamer.PlayerLabel') | The text label instance to destroy. |

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-GetAllDynamicTextLabels'></a>
### GetAllDynamicTextLabels() `method`

##### Summary

Get all created dynamic textlabels.

##### Returns

A list of dynamic textlabels.

##### Parameters

This method has no parameters.

<a name='M-Altv_Roleplay-EntityStreamer-TextLabelStreamer-GetDynamicTextLabel-System-UInt64-'></a>
### GetDynamicTextLabel(dynamicTextLabelId) `method`

##### Summary

Get a dynamic text label by it's ID.

##### Returns

The dynamic textlabel or null if not found.

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| dynamicTextLabelId | [System.UInt64](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.UInt64 'System.UInt64') | The ID of the textlabel. |

<a name='T-Global-mGlobal-VirtualAPI'></a>
## VirtualAPI `type`

##### Namespace

Global.mGlobal

<a name='M-Global-mGlobal-VirtualAPI-RunThreadSafe-System-Action-'></a>
### RunThreadSafe(function) `method`

##### Summary

Permet d'effectuer un appel API 100% Thread-Safe via le taskmanager interne de alt:V

##### Parameters

| Name | Type | Description |
| ---- | ---- | ----------- |
| function | [System.Action](http://msdn.microsoft.com/query/dev14.query?appId=Dev14IDEF1&l=EN-US&k=k:System.Action 'System.Action') |  |
