# CARTO mobile SDK style editor app
Minimal mobile app to preview/fix base map style CSS with mobile SDK

Cross-platform, done with Xamarin. Meant for tablets, may work on phones.

## Development plan/features

_draft plan:_

1. Test Xamarin Forms text editor field - is it usable? if yes, use xamarin forms. if no, use native text control

2. Style Viewer 
  - tablet-oriented UI - left map, right css editor field
  - read style css from style.zip package, show list of css (mss) files, selected file contents in text editor, show map in map view

3. Minimal editor:
 - enable changing css, 
 - save style button -> updates zipfile, reconfigures map to show changed style

4. Next features
 - change tile source. Start with hardcoded nutiteq.osm, enable changing it as text field to use OMT, mapzen etc.
 - support connect/browse/read/write file from Google Drive

5. "Maybe future"
 - css style highlighting
 - css style helpers (precomplete, color selectors etc)
 - xml style editor
 - build as desktop app for Windows 10 and/or OSX (Xamarin Forms supports that in principle)
