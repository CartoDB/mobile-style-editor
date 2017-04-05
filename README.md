## CARTO Mobile SDK style editor app

Minimal mobile app to preview/fix base map style CSS with mobile SDK

Cross-platform, done with Xamarin. Meant for tablets, may work on phones.

### Development plan/features
___________

_Draft plan:_

1. <del>Test Xamarin Forms text editor field - is it usable? if yes, use xamarin forms. if no, use native text control</del> **Forms Entry field does not support formatted text. Still doing it in Forms, but need to use native text view**

2. Style Viewer 
  - Tablet-oriented UI - left map, right css editor field
  - Read style css from style.zip package, show list of css (mss) files, selected file contents in text editor, show map in map view

3. Minimal editor:
 - Enable changing css, 
 - Save style button -> updates zip file, reconfigures map to show changed style

4. Next features
 - Change tile source. Start with hardcoded nutiteq.osm, enable changing it as text field to use OMT, mapzen etc.
 - Support connect/browse/read/write file from Google Drive

5. "Maybe future"
 - CSS style highlighting
 - CSS style helpers (precomplete, color selectors etc)
 - Xml style editor
 - Build as desktop app for Windows 10 and/or OSX (Xamarin Forms supports that in principle)
