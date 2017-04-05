# CARTO mobile SDK style editor app
Minimal mobile app to preview/fix base map style CSS with mobile SDK

Cross-platform, done with Xamarin. Meant for tablets, may work on phones.

## Development plan/features

_draft plan:_

1. test xamarin forms text editor field - is it usable. if yes, use xamarin forms. if no, use native text control
2. style viewer: 
  - tablet-oriented UI - left map, right css editor field
  - read style css from style.zip package, show list of css (mss) files, selected file contents in text editor, show map in map view
3. minimal editor:
 - enable changing css, 
 - save style button -> updates zipfile, reconfigures map to show changed style
4. Next features
 - change tile source. Start with hardcoded nutiteq.osm
 - support connect/browse/read/write file from Google Drive
