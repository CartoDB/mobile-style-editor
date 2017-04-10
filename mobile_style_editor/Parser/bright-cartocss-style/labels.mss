// =====================================================================
// LABELS

// General notes:
// - `text-halo-rasterizer: fast;` gives a noticeable performance
//    boost to render times and is recommended for *all* halos.

// ---------------------------------------------------------------------
// Languages

// There are 5 language options in the MapBox Streets vector tiles:
// - Local/default: '[name]'
// - English: '[name_en]'
// - French: '[name_fr]'
// - Spanish: '[name_es]'
// - German: '[name_de]'
@name: '[name]';  

// ---------------------------------------------------------------------
// Fonts
@fallback: 'Noto Sans Regular';
@xfont1: 'HanWangHeiLight Regular';
@xfont2: 'NanumBarunGothic Regular';
//@xfont3: 'Unifont Medium';
@sans: 'Open Sans Regular', @fallback, @xfont1, @xfont2;
@sans_md: 'Open Sans Semibold', @fallback, @xfont1, @xfont2;
@sans_bd: 'Open Sans Bold', @fallback, @xfont1, @xfont2;
@sans_it: 'Open Sans Italic', @fallback, @xfont1, @xfont2;

// ---------------------------------------------------------------------
// Countries/States/Islands
#country_label[type='country'][zoom>=3][zoom<10],
#country_label[scalerank=2][zoom>=3][zoom<10]{
  text-placement: [nuti::texts3d];
  text-name: @name;
  text-face-name: @sans_bd;
  text-transform: uppercase;
  text-wrap-width: 100;
  text-wrap-before: true;
  text-fill: @country_label;
  text-halo-fill: fadeout(#fff, 20%);
  text-halo-radius: 1.5;
  text-line-spacing: -4;
  text-character-spacing: 0.5;
  text-size: 10;
  [zoom>=4] {text-size: 11;}
  [zoom>=5] {text-size: 13;}
  [zoom>=6] {text-size: 14;}
  [zoom>=7] {text-size: 15;}
  [zoom>=8] {text-size: 16;}
}

#country_label[type='state'][zoom>=6][zoom<=11],
#country_label[scalerank=0][zoom>=6][zoom<=11]{
  text-placement: [nuti::texts3d];
  text-name: @name;
  text-face-name: @sans_bd;
  text-transform: uppercase;
  text-wrap-width: 40;
  text-fill: @country_label;
  text-halo-fill: fadeout(#fff, 20%);
  text-halo-radius: 1.2;
  text-line-spacing: -4;
  text-character-spacing: 0.5;
  text-size: 9;
  [zoom>=6] {text-size: 10;}
  [zoom>=7] {text-size: 11;}
  [zoom>=8] {text-size: 12;}
  [zoom>=10] {text-size: 14;}
}

#country_label[type='island'][zoom>=8] {
  text-placement: [nuti::texts3d];
  text-name: @name;
  text-face-name: @sans_bd;
  text-transform: uppercase;
  text-wrap-width: 30;
  text-wrap-before: true;
  text-fill: @country_label;
  text-halo-fill: fadeout(#fff, 20%);
  text-halo-radius: 1.2;
  text-line-spacing: -4;
  text-character-spacing: 0.5;
  text-size: 11;
  [zoom>=9] {text-size: 11;}
  [zoom>=10] {text-size: 12;}
}

// ---------------------------------------------------------------------

// ---------------------------------------------------------------------
// Cities, towns, villages, etc
// City labels with dots for low zoom levels.
// The separate attachment keeps the size of the XML down.
#place_label::citydots[type='city'][zoom>=5][zoom<=7] {
  [ldir='N'],[ldir='S'],[ldir='E'],[ldir='W'],
  [ldir='NE'],[ldir='SE'],[ldir='SW'],[ldir='NW'] {
    shield-file: url("shield/dot-small.png");
    shield-placement: [nuti::shields3d];
    shield-unlock-image: true;
    shield-name: @name;
    shield-size: 12;
    shield-face-name: @sans;
    shield-fill: @city_label;
    shield-halo-fill: fadeout(#fff, 30%);
    shield-halo-radius: 1.2;
    [zoom=7] { shield-size: 14; }
    [ldir='E'] { shield-text-dx: 5; }
    [ldir='W'] { shield-text-dx: -5; }
    [ldir='N'] { shield-text-dy: -5; }
    [ldir='S'] { shield-text-dy: 5; }
    [ldir='NE'] { shield-text-dx: 4; shield-text-dy: -4; }
    [ldir='SE'] { shield-text-dx: 4; shield-text-dy: 4; }
    [ldir='SW'] { shield-text-dx: -4; shield-text-dy: 4; }
    [ldir='NW'] { shield-text-dx: -4; shield-text-dy: -4; }
  }
}

#place_label { 
  [type='city'][zoom>=8][zoom<=15] {
  	text-placement: [nuti::texts3d];
    text-name: @name;
    text-face-name: @sans_md;
    text-wrap-width: 120;
    text-fill: @city_label;
    text-halo-fill: fadeout(#fff, 30%);
    text-halo-radius: 1.2;
    text-size: 18;
    [zoom>=10] {text-wrap-width: 140;}
    [zoom>=12] {text-size: 24; text-wrap-width: 180;}
  }
  [type='town'][zoom>=8][zoom<=15] {
    text-placement: [nuti::texts3d];
    text-name: @name;
    text-face-name: @sans_md;
    text-wrap-width: 120;
    text-fill: @city_label;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    text-size: 13;
    [zoom>=10] { text-size: 15; }
    [zoom>=12] { text-size: 16; }
    [zoom>=14] { text-size: 20; }
    [zoom>=16] { text-size: 24; }
  }
  [type='village'][zoom>=10] {
    text-placement: [nuti::texts3d];
    text-size: 9;
    text-name: @name;
    text-wrap-width: 40;
    text-fill: @village_label;
    text-face-name:	@sans_md;
    text-transform: uppercase;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    [zoom>=12] { text-size: 11}
    [zoom>=13] { text-size: 12}
    [zoom>=14] { text-size: 13}
    [zoom>=17] { text-size: 14}
  }
  [type='hamlet'][zoom>=10] {
    text-placement: [nuti::texts3d];
    text-name: @name;
    text-face-name: @sans;
    text-wrap-width: 400;
    text-wrap-before: true;
    text-fill: @city_label;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    text-size: 10;
    [zoom>=12] { text-size: 12}
    [zoom>=13] { text-size: 13}
    [zoom>=14] { 
      text-size: 13;  
      text-face-name: @sans_md;
    }
    [zoom>=15] { text-size: 14; }
    [zoom>=16] { text-size: 16;}
  }
  [type='suburb'][zoom>=12] {
    text-placement: [nuti::texts3d];
    text-name: @name;
    text-face-name: @sans_it;
    text-wrap-width: 120;
    text-wrap-before: true;
    text-fill: @village_label;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    text-size: 12;
    [zoom>=12] { text-size: 10}
    [zoom>=14] { text-size: 12}
    [zoom>=16] { text-size: 14}
  }
  [type='neighbourhood'][zoom>=13] {
    text-placement: [nuti::texts3d];
    text-name: @name;
    text-fill: @village_label;
    text-face-name:	@sans_md;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    text-size: 11;
    text-wrap-width: 40;
    [zoom>=17] { text-size: 15; }
  }
}

// ---------------------------------------------------------------------
// Points of interest
#poi_label[maki!=null][zoom>=10][scalerank<=1],
#poi_label[maki!=null][zoom=15][scalerank<=2],
#poi_label[maki!=null][zoom=16][scalerank<=5],
#poi_label[maki!=null][zoom=17][scalerank<=6],
#poi_label[maki!=null][zoom>=18] {
  ::icon {
    marker-placement: [nuti::markers3d];
    //marker-fill:#666;
    marker-file:url('icon/[maki]-12.png');
    marker-width: 10;
    //marker-line-opacity: 4;
    [zoom>=16] {marker-width: 11;}
    [zoom>=17] {marker-width: 12;}
  }
  ::label[zoom<=15] {
    [maki='park'],
    [maki='rail'],
    [maki='rail-light'],
    [maki='airport'],
    [maki='ferry'],
    [maki='harbor'],
    [maki='bus'] {
      text-name: '';
      text-face-name: @sans;
    }
  }
  ::label[zoom>=15] {
      text-placement: [nuti::texts3d];
      text-name: @name;
      text-dy: 8;
      text-face-name: @sans_md;
      text-size: 9;
      text-fill: @poi_label;
      text-halo-fill: fadeout(#fff, 20%);
      text-halo-radius: 1.2;
      text-wrap-width: 50;
      text-line-spacing: -4;
      [zoom>=16] { text-wrap-width: 40; }
  }
}

// ---------------------------------------------------------------------
// Roads

#road_label[reflen>=1][reflen<=6][zoom>=9][zoom<=17]::shield {
  shield-placement: [nuti::shields3d];
  shield-name: [ref];
  shield-face-name: @sans_bd;
  shield-fill: @road_label;
  shield-size: 8;
  shield-file: url('shield/motorway_sm_[reflen].png');
  
  /*
  shield-min-distance: 100;
  shield-min-padding: 100;
  shield-spacing: 100;
  [zoom>=15] {
    shield-size: 9;
    shield-file: url('shield/motorway_lg_[reflen].png');
    shield-min-distance: 250; shield-spacing: 250;
    shield-min-padding: 250;
  }
  */
  //NB! These attributes are different in MB Studio and Nutiteq SDK
  //These adjusted for Nutiteq SDK
  //---------
  
  shield-spacing: 170;
  shield-min-distance: 170;
  [zoom>=15] {
    shield-size: 9;
    shield-file: url('shield/motorway_lg_[reflen].png');
    shield-min-distance: 250; 
    shield-spacing: 250;
  }
  
  //---------  
  
}

#road_label[zoom>=12] {
  [class='main'] {
    text-name: @name;
    text-face-name: @sans;
    text-placement: line;  // text follows line path
    text-fill: @road_label;
    text-halo-fill: fadeout(#fff, 30%);
    text-halo-radius: 1.2;
    text-spacing: 40;
    text-size: 9;
    [zoom>=13] {text-size: 10;}
    [zoom>=14] { 
      text-halo-radius: 1.5;
      text-spacing: 50;
    }
    [zoom>=15] {
      text-spacing: 80;
    }
    [zoom>=18] {
       text-size: 12;
    }
  }  
}

#road_label[zoom>=14] {
  [class='street'],
  [class='path'] {
    text-name: @name;
    text-face-name: @sans;
    text-placement: line;  // text follows line path
    text-fill: @road_label;
    text-halo-fill: fadeout(#fff, 20%);
    text-halo-radius: 1.2;
    text-spacing: 40;
    text-size: 9;
    [zoom>=16] {
      text-halo-fill: fadeout(#fff, 30%);
      text-size: 10;
      text-spacing: 80;
    }
    [zoom>=18] {
      text-size: 12;
    }
  } 
}

#route_label {
    text-name: @name;
    text-face-name: @sans;
    text-placement: line;  // text follows line path
    text-fill: @route_label;
    text-halo-fill: fadeout(#fff, 40%);
    text-halo-radius: 1.2;   
    text-character-spacing: 1.2;
    text-avoid-edges: true;  // prevents clipped labels at tile edges
    text-size: 9;
    [zoom>=13] { text-size: 10; }
    [zoom>=18] { text-size: 12; }
}

// ---------------------------------------------------------------------
// Water
#marine_label {
  text-name: @name;
  text-placement: [nuti::texts3d];
  text-face-name: @sans_it;
  text-wrap-width: 60;
  text-fill: @water_label;
  text-halo-fill: fadeout(#fff, 70%);
  text-halo-radius: 1.2;
  text-size: 14;
  text-character-spacing: 1;
}

#water_label {
  [zoom<12],
  [zoom>=13][area>500000],
  [zoom>=14][area>200000],
  [zoom>=15][area>100000],
  [zoom>=16][area>10000],
  [zoom>=17] {
    text-name: @name;
    text-placement: point;
    text-face-name: @sans_it;
    text-size: 10;
    text-wrap-width: 100;
    text-fill: @water_label;
    text-halo-fill: fadeout(#fff, 70%);
    text-halo-radius: 1.2;
    [zoom>=12] { text-size: 11; }
    [zoom>=15] { text-size: 12; }
  }
}

#waterway_label {
  [zoom>=12] {
    text-name: @name;
    text-face-name: @sans_it;
    text-fill: @water_label;
    text-size: 9;
    text-halo-fill: fadeout(#fff, 70%);
    text-halo-radius: 1.2;
    text-placement: line;
    text-spacing: 60;
    [zoom>=13] { 
      text-spacing: 80; 
    }
    [zoom>=14] { text-size: 11; }   
  }
}

#area_label {
  [zoom>=10][area > 40000000],
  [zoom>=12][area > 4000000],
  [zoom>=13][area > 1500000],
  [zoom>=14][area > 500000],
  [zoom>=15][area > 40000],
  [zoom>=16][area > 4000],
    {
    text-placement: [nuti::texts3d];
    text-name: @name;
    text-face-name: @sans_it;
    text-fill: @area_label;
    [type='military'] {
      text-fill: @military_label;
    }
    [type='quarry'] {
      text-fill: @quarry_label;
    }
    [type='beach'] {
      text-fill: @beach_label;
    }
    text-size: 10;
    text-halo-fill: fadeout(#fff, 50%);
    text-halo-radius: 1.2;
    [zoom>=15] { text-size: 11; }
    [zoom>=17] { text-size: 12; }
  }
}

// ---------------------------------------------------------------------
// House numbers
#housenum_label[zoom>=17] {
  text-placement: [nuti::texts3d];
  text-name: [house_num];
  text-face-name: @sans_it;
  text-fill: @houseno_label;
  text-size: 8;
  text-dy: 0;
  text-dx: 0;
  [zoom>=18] { text-size: 10; }
}

#barrier_line_label[zoom>=15] {
    text-name: @name;
    text-face-name: @sans_it;
    text-placement: line;  // text follows line path
    text-fill: @barrier_line_label;
    text-halo-fill: fadeout(#fff, 40%);
    text-halo-radius: 1.2;   
    text-dy: 3;
    text-size: 8;
}