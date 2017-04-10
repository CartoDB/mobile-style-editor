// ---------------------------------------------------------------------
// Common Colors
//@land: #f8f1eb;
@land: #f2f2eb;
@wood: #5fb534;
@water: #80b7ed;
@waterway_color: @water * 0.9;
@sand: mix(#fbf385, @land, 73%);
@meadow: mix(#e7e8d8, @sand, 95%);
@grass: mix(@park, @sand, 76%);
@military: #fa5012;
@military_line: mix(@land, @military, 15%);
@quarry: darken(mix(@land, #facf62, 70%), 6%);
@quarry_line: darken(mix(@land, @quarry, 15%), 13%);
@landfill: darken(mix(@land, #cec1b8, 70%), 6%);
@park: #e0ebc5;
@hospital: #fde;
@school: #ece1f8;
@parking: darken(mix(@land, #fbf385, 77%), 3%);
@cemetery: mix(@park, #ddd, 25%);
@industrial: darken(mix(@land, #eee, 30%), 2%);
@residential: darken(mix(@land, #eee, 70%), 1.6%);
@aeroway: @land * 0.96;
@aeroway_line: @land * 0.89;

@building: mix(@land, #9a300c, 88%);
@building_contour: mix(@land, #000, 75%);
@3d_building_fill: #d3cfcc;
@3d_building_contour: #d3cfcc;

@admin_line: #446;
@admin_line_maritime: mix(@water, #2654a3, 95%);
@contourline: #666;

// ---------------------------------------------------------------------
// Road colors
@motorway:          #e08f73;
@main:              #f4d992;
@street:            #fff;
@tertiary:          #fbf099;
@street_limited:    #f3f3f3;
@motorway_line:     mix(@motorway, #800, 75%);
@main_line:         mix(@main, #800, 65%);
@tertiary_line:     mix(@main, #a09c9c, 5%);
@street_line:       @land * 0.7;
@path:              #bba;
@track:             #f658f2;
@steps:             #bba;
@major_rail:        #aaa;
@siding_rail:       #c9c8c8;
@tram:              #cba;

@aerialway: mix(@land, #181c8a, 65%);

@hiking_route:      #f1a085;
@ski_route:         #626cf1;
@ferry_route:       mix(@water, #ce4f24, 85%);

// ---------------------------------------------------------------------
// Label colors
@country_label:  #334;
@city_label:  #333;
@village_label:  #633;
@poi_label:  #666;
@road_label: #765;
@route_label: #765;

@water_label: darken(@water, 30%);
@area_label: darken(@land, 45%);
@quarry_label: darken(@quarry, 38%);
@beach_label: darken(@sand, 38%);
@military_label: darken(@military, 18%);

@houseno_label: #888;
@barrier_line_label: #a96621;

// ---------------------------------------------------------------------
Map {
  background-color:@land; font-directory: url(fonts/);
}

// ---------------------------------------------------------------------
// Political boundaries
#admin {
  line-opacity: 0.5;
  line-join: round;
  line-cap: round;
  line-color: @admin_line;
  [maritime=1] {
    line-color: @admin_line_maritime;
  }
  // Countries
  [admin_level=2] {
    /*
    [zoom>=2] { line-width: 0.5; }
    [zoom>=3] { line-width: 0.8; }
    [zoom>=5] { line-width: 1; }
    [zoom>=7] { line-width: 2; }
    [zoom>=8] { line-width: 3; }
    [zoom>=9] { line-width: 4; }
    */
    [disputed=1] { line-dasharray: 4,4; }
    line-width: linear([view::zoom], (2, 0.5), (3, 0.8), (5, 1), (7, 2), (8, 3), (9, 4));
  }
  // States / Provices / Subregions
  [admin_level>=3] {
    line-dasharray: 5,3,3,3;
    /*
    [zoom>=3] { line-width: 0.3; }
    [zoom>=5] { line-width: 0.4; line-dasharray: 8,3,3,3;}
    [zoom>=6] { line-width: 0.6; }
    [zoom>=7] { line-width: 0.8; }
    [zoom>=8] { line-width: 1.2; line-dasharray: 10,3,3,3;}
    [zoom>=12] { line-width: 2; }
    */
    [zoom>=5] { line-dasharray: 8,3,3,3;}
    [zoom>=8] { line-dasharray: 10,3,3,3;}
    line-width: linear([view::zoom], (3, 0.3), (5, 0.4), (6, 0.6), (7, 0.8), (8, 1.2), (12, 2));
  }
}

// ---------------------------------------------------------------------
// Water Features 
#water {
  //polygon-fill: @water - #111;
  //opacity: 0.5;
  // Map tiles are 256 pixels by 256 pixels wide, so the height 
  // and width of tiling pattern images must be factors of 256.
  polygon-pattern-file: url(pattern/water.png);
}

#waterway {
  line-color: @waterway_color;
  line-cap: round;
  line-width: 0.3;
  [class='river'] {
    /*
    [zoom>=9] { line-width: 0.5; }
    [zoom>=12] { line-width: 1; }
    [zoom>=14] { line-width: 2; }
    [zoom>=16] { line-width: 3; }
    */
    line-width: linear([view::zoom], (0, 0.3), (9, 0.5), (12, 1), (14, 2), (16, 3));
  }
  [class='stream'],
  [class='stream_intermittent'],
  [class='canal'] {
    /*
    [zoom>=14] { line-width: 0.75; }
    [zoom>=16] { line-width: 1; }
    [zoom>=18] { line-width: 3; }
    */
    line-width: linear([view::zoom], (0, 0.3), (14, 0.75), (16, 1), (18, 3));
  }
  [class='stream_intermittent'] { line-dasharray: 6,2,2,2; }
}

// ---------------------------------------------------------------------
// Landuse areas 
#landuse {
  // Land-use and land-cover are not well-separated concepts in
  // OpenStreetMap, so this layer includes both. The 'class' field
  // is a highly opinionated simplification of the myriad LULC
  // tag combinations into a limited set of general classes.
  //[class='wood'] { polygon-fill: @wood;}
  
  [class='landfill'] { 
    polygon-fill: @landfill;
    polygon-opacity:0.6;
  }
  [class='quarry'] { 
    polygon-fill: @quarry;
    polygon-opacity:0.6;
    ::line {
      line-width: 0.5;
      line-color: @quarry_line;
    }
  }
  [class='military'] { 
    polygon-fill: @military; 
    polygon-opacity:0.1;
    ::line {
      line-width: 0.5;
      line-color: @military_line;
    }
  }
  
  [class='meadow'] { polygon-fill: @meadow;}
  [class='grass'] { polygon-fill: @grass;}
  [class='park'] { polygon-fill: @park;}
  [class='parking'] { polygon-fill: @parking; }
  [class='nature_park'] { polygon-fill: @park; }
  [class='cemetery'] { polygon-fill: @cemetery }
  [class='hospital'] { polygon-fill: @hospital }
  [class='school'] { polygon-fill: @school }
  [class='industrial'] { polygon-fill: @industrial}
  [class='residential'] { polygon-fill: @residential}
  [class='sand'] { polygon-fill: @sand; }
  ::overlay {
    // Landuse classes look better as a transparent overlay.
    [class='wood'] { 
      polygon-fill: @wood;
      polygon-opacity: 0.13; 
    }
  }
}

#landuse_overlay {
  [class='wetland'][zoom>=7] {
    polygon-pattern-file:url(pattern/wetland.png); 
  }
}
  
// ---------------------------------------------------------------------
// Buildings 
#building [zoom>=14][zoom<=17]{
  // At zoom level 13, only large buildings are included in the
  // vector tiles. At zoom level 14+, all buildings are included.
  ::roof {
    polygon-fill: @building;
  }
  ::contour[zoom=17] {
    line-width: 0.5;
    line-color: @building_contour;
  }
}
// Seperate attachments are used to draw buildings with depth
// to make them more prominent at high zoom levels
#building [zoom>=18]{
  ::3d['nuti::buildings3d'=true] {     
    building-height: "[nuti-height]"; //only for nutiteq SDK3
    building-fill: @3d_building_fill;
    building-fill-opacity: 0.25;
  } 
  ::roof['nuti::buildings3d'=false] {
    polygon-fill: @building;
  }
  ::contour['nuti::buildings3d'=false] { 
    line-width: 1;
    line-color: @3d_building_contour;
  } 
}

// ---------------------------------------------------------------------
// Aeroways 
#aeroway [zoom>=10] {
  ::line['mapnik::geometry_type'=2] {
    line-color: @aeroway_line;
    [type='runway'] { line-width: 5; }    
    [type='taxiway'] {  
      line-width: 1;
      [zoom>=15] { line-width: 2; }
    }
  }    
  ::fill['mapnik::geometry_type'=3] {
    polygon-fill: @aeroway;  
  }
}

// =====================================================================
// BARRIERS
// =====================================================================
#barrier_line[zoom>=12][class='cliff'] {
  line-pattern-file: url(pattern/cliff-lg_rot.png);
}


