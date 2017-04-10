
// ---------------------------------------------------------------------
// Roads are split across 3 layers: #road, #bridge, and #tunnel. Each
// road segment will only exist in one of the three layers. The
// #bridge layer makes use of Mapnik's group-by rendering mode;
// attachments in this layer will be grouped by layer for appropriate
// rendering of multi-level overpasses.
// The main road style is for all 3 road layers and divided into 2 main
// attachments. The 'case' attachment is 

#aerialway {
  line-color: @aerialway;
  line-width:0.5;
  h/line-width: 3;
  h/line-color: @aerialway;
  h/line-dasharray: 0.5, 15;  
}

#road, #bridge, #tunnel {
  // casing/outlines & single lines
  ::case[zoom>=6] {
    [class='motorway'] {
      line-join:round;
      line-color: @motorway_line;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      /*
      [zoom>=6]  { line-width:0.4; }
      [zoom>=7]  { line-width:0.6; }
      [zoom>=8] { line-width:2.5; }
      [zoom>=10]  { line-width:3; }
      [zoom>=11] { line-width:3.5; }
      [zoom>=14] { line-width:5; }
      [zoom>=15] { line-width:5.6; }
      [zoom>=16] { line-width:7; }
      [zoom>=17] { line-width:8; }
      [zoom>=18] { line-width:10; }
      */
      line-width: linear([view::zoom], (6, 0.4), (7, 0.6), (8, 2.5), (10, 3), (11, 3.5), (14, 5), (15, 5.6), (16, 7), (17, 8), (18, 10), (24, 60));
    }
    [class='motorway_link'][zoom>=11] {
      line-join:round;
      line-color: @motorway_line;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      /*
      [zoom>=11] { line-width:2.6; }
      [zoom>=14] { line-width:3; }
      [zoom>=15] { line-width:5; }
      [zoom>=16] { line-width:6; }
      */
      line-width: linear([view::zoom], (11, 2.6), (14, 3), (15, 5), (16, 6), (18, 8), (24, 48));
    }
    [class='main'] {
      line-join:round;
      line-color: @main_line;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      /*
      [zoom>=6] { line-width:0.2; }
      [zoom>=7] { line-width:0.5; }
      [zoom>=8] { line-width:2.2; }
      [zoom>=10] { line-width:2.8; }
      [zoom>=11] { line-width:3.5; }
      [zoom>=13] { line-width:4; }  
      [zoom>=14] { line-width:4.5; }
      [zoom>=15] { line-width:5.6; }
      [zoom>=16] { line-width:7; }
      [zoom>=17] { line-width:8; }
      [zoom>=18] { line-width:10; }
      */
      line-width: linear([view::zoom], (6, 0.2), (7, 0.5), (8, 2.2), (10, 2.8), (11, 3.5), (13, 4), (14, 4.5), (15, 5.6), (16, 7), (17, 8), (18, 10), (24, 60));
    }
    [class='main_link'][zoom>=11] {
      line-join:round;
      line-color: @main_line;
      //#road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      /*
      [zoom>=11] { line-width:2.4; }
      [zoom>=14] { line-width:3; }
      [zoom>=15] { line-width:5; }
      [zoom>=16] { line-width:6; }
      [zoom>=17] { line-width:7; }
      */
      line-width: linear([view::zoom], (11, 2.4), (14, 3), (15, 5), (16, 6), (17, 7), (18, 8), (24, 48));
    }
    [class='secondary'] {
      line-width: 0.7;
      line-join:round;
      line-color: @main_line;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
    }
    [class='tertiary'] {
      line-join:round;
      line-color: @tertiary_line;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      /*
      [zoom>=9] { line-width:1.4; }
      [zoom>=10] { line-width:2.8; }
      [zoom>=11] { line-width:3.5; }
      [zoom>=13] { line-width:4; }  
      [zoom>=14] { line-width:4.5; }
      [zoom>=15] { line-width:5.6; }
      [zoom>=16] { line-width:7; }
      [zoom>=17] { line-width:8; }
      */
      line-width: linear([view::zoom], (9, 1.4), (10, 2.8), (11, 3.5), (13, 4), (14, 4.5), (15, 5.6), (16, 7), (17, 8), (18, 10), (24, 60));
    }
     
    [class='street'][zoom>=11] { 
      line-join: round;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      line-color: @street_line;
      /*
      [zoom>=14] { line-width: 3.5; }
      [zoom>=15] { line-width:3.5; }
      [zoom>=16] { line-width:6; }
      [zoom>=17] { line-width:7; }
      [zoom>=18] { line-width:8; }
      */
      line-width: linear([view::zoom], (11, 0), (11.5, 0.5), (13, 0.6), (14, 3.5), (15, 3.5), (16, 6), (17, 7), (18, 8), (24, 48));
    }

    [class='street_limited'][zoom>=11] { 
      line-join:round;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      line-color: @street_line;
      /*
      [zoom>=14] { line-width: 3.5; }
      [zoom>=15] { line-width:3.5; }
      [zoom>=16] { line-width:6; }
      [zoom>=17] { line-width:7; }
      [zoom>=18] { line-width:8; }
      */
      line-width: linear([view::zoom], (11, 0), (11.5, 0.5), (13, 0.6), (14, 3.5), (15, 3.5), (16, 6), (17, 7), (18, 8), (24, 48));
    }
 
    [class='service'][zoom>=11] { 
      line-join: round;
      #road { line-cap: round; }
      #tunnel { line-dasharray:3,2; }
      line-color: @street_line;
      /*
      [zoom>=14] { line-width:3.5; }
      [zoom>=15] { line-width:3.5; }
      [zoom>=16] { line-width:4; }
      [zoom>=17] { line-width:4; }
      [zoom>=18] { line-width:6; }
      */
      line-width: linear([view::zoom], (11, 0), (11.5, 0.5), (13, 0.6), (14, 3.5), (15, 3.5), (16, 4), (17, 4), (18, 6), (24, 36));
    }
       
    [class='path'][zoom>=12],
    [class='pedestrian'][zoom>=12] {
      #road, #tunnel {
        line-join:round;
        #road { line-cap: round; }
        #tunnel { line-dasharray: 3,2; } 
        line-color: @path;
        line-dasharray: 3,2;
        [zoom>=16] { line-dasharray: 4,3; }
        /*
        [zoom>=12] { line-width: 0.7; }
        [zoom>=14] { line-width: 0.8; }
        [zoom>=16] { line-width: 1.2; }
        [zoom>=17] { line-width: 1.5; }
        */
        line-width: linear([view::zoom], (12, 0.7), (14, 0.8), (16, 1.2), (17, 1.5));
      }
    }
    
    [class='path'][zoom>=12],
    [class='pedestrian'][zoom>=12] {
      #bridge {
        line-join:round;
        line-cap: round;
        line-color: @street_line;
        /*
        [zoom>=12] { line-width: 2.4; }
        [zoom>=15] { line-width: 2.7; }
        [zoom>=16] { line-width: 3; }
        [zoom>=17] { line-width: 3.6; }
        [zoom>=18] { line-width: 4.8; }
        */
        line-width: linear([view::zoom], (12, 2.4), (15, 2.7), (16, 3), (17, 3.6), (18, 4.8));
      }
    }
    
    [class='track'][zoom>=14] {
      line-color: @track;
      line-dasharray: 2,1;
      [zoom>=16] { line-width: 1.2; }
      [zoom>=17] { line-width: 1.5; }
    }
    [class='steps'][zoom>=16] {
      ::case {
        line-color: @land;
        line-opacity: 0.5;
        line-join: round;
        line-width: 6;
      }
      ::fill {
        line-color: @steps;
        line-width: 4;
        line-dasharray: 2,1;
      }
    }
  }
  
  // fill/inlines
  ::fill[zoom>=6] {
    [class='motorway'][zoom>=8] {
      line-join: round;
      #road, #bridge { line-cap: round; }
      line-color: @motorway;
      #tunnel { line-color: lighten(@motorway, 4%); }
      /*
      [zoom>=8] { line-width: 0.5; }
      [zoom>=10] { line-width: 1; }
      [zoom>=11] { line-width: 2; }
      [zoom>=14] { line-width: 3.5; }
      [zoom>=15] { line-width: 4.6; }
      [zoom>=16] { line-width: 6; }
      [zoom>=17] { line-width: 7; }
      [zoom>=18] { line-width: 8.6; }
      */
      line-width: linear([view::zoom], (8, 0.5), (10, 1), (11, 2), (14, 3.5), (15, 4.6), (16, 6), (17, 7), (18, 8.6), (24, 51.6));
    }
    [class='motorway_link'][zoom>=11] {
      line-join: round;
      #road, #bridge { line-cap: round; }
      line-color: @motorway;
      #tunnel { line-color: lighten(@motorway, 4%); }
      /*
      [zoom>=11] { line-width: 1.4; }
      [zoom>=14] { line-width: 1.9; }
      [zoom>=15] { line-width: 3.6; }
      [zoom>=16] { line-width: 4.5; }
      */
      line-width: linear([view::zoom], (11, 1.4), (14, 1.9), (15, 3.6), (16, 4.5), (18, 5.5), (24, 33));
    }
    [class='main'][zoom>=8] {
      line-join: round;
      #road, #bridge { line-cap: round; }
      line-color: @main;
      #tunnel { line-color: lighten(@main, 4%); }
      /*
      [zoom>=8] { line-width: 1.5; }
      [zoom>=10] { line-width: 2; }
      [zoom>=11] { line-width: 2.8; }
      [zoom>=13] { line-width: 3.2; }
      [zoom>=14] { line-width: 3.6; }
      [zoom>=15] { line-width: 4.6; }
      [zoom>=16] { line-width: 6; }
      [zoom>=17] { line-width: 7; }
      [zoom>=18] { line-width: 8.6; }
      */
      line-width: linear([view::zoom], (8, 1.5), (10, 2), (11, 2.8), (13, 3.2), (14, 3.6), (15, 4.6), (16, 6), (17, 7), (18, 8.6), (24, 51.6));
    }
    [class='main_link'][zoom>=11] {
      line-join: round;
      //#road, #bridge { line-cap: round; }
      line-color: @main;
      #tunnel { line-color: lighten(@main, 4%); }
      /*
      [zoom>=11] { line-width: 1.8; }
      [zoom>=14] { line-width: 2.4; }
      [zoom>=15] { line-width: 3.8; }
      [zoom>=16] { line-width: 5; }
      [zoom>=17] { line-width: 6; }
      */
      line-width: linear([view::zoom], (11, 1.8), (14, 2.4), (15, 3.8), (16, 5), (17, 6), (18, 7), (24, 42));
    }
    [class='tertiary'] {
      line-join: round;
      line-color: @tertiary;
      #road, #bridge { line-cap: round; }
      /*
      [zoom>=9] { line-width: 0.9; }
      [zoom>=10] { line-width: 2; }
      [zoom>=11] { line-width: 2.8; }
      [zoom>=13] { line-width: 3.4; }
      [zoom>=14] { line-width: 3.6; }
      [zoom>=15] { line-width: 4.6; }
      [zoom>=16] { line-width: 6; }
      [zoom>=17] { line-width: 7; }
      */
      line-width: linear([view::zoom], (9, 0.9), (10, 2), (11, 2.8), (13, 3.4), (14, 3.6), (15, 4.6), (16, 6), (17, 7), (18, 8), (24, 48));
    }

    [class='street'][zoom>=13] {
      line-join: round;
      #road, #bridge { line-cap: round; }
      line-color: @street;
      /*
      [zoom>=14] { line-width: 2.6; }
      [zoom>=15] { line-width: 2.8; }
      [zoom>=16] { line-width: 5; }
      [zoom>=17] { line-width: 6; }
      [zoom>=18] { line-width: 7; }
      */
      line-width: linear([view::zoom], (13, 0), (14, 2.6), (15, 2.8), (16, 5), (17, 6), (18, 7), (24, 42));
    }
    
    [class='street_limited'][zoom>=13] {
      line-join: round;
      #road { line-cap: round; }
      line-color: @street_limited;
      /*
      [zoom>=14] { line-width: 2.6; }
      [zoom>=15] { line-width: 2.8; }
      [zoom>=16] { line-width: 5; }
      [zoom>=17] { line-width: 6; }
      [zoom>=18] { line-width: 7; }
      */
      line-width: linear([view::zoom], (13, 0), (14, 2.6), (15, 2.8), (16, 5), (17, 6), (18, 7), (24, 42));
    }
    
    [class='service'][zoom>=13] {
      line-join: round;
      line-color: @street;
      #road, #bridge { line-cap: round; }
      /*
      [zoom>=14] { line-width: 2.6; }
      [zoom>=16] { line-width: 3; }
      [zoom>=18] { line-width: 5; }
      */
      line-width: linear([view::zoom], (13, 0), (14, 2.6), (16, 3), (18, 5), (24, 30));
    }
    
    [class='path'][zoom>=12],
    [class='pedestrian'][zoom>=12] {
      #bridge { 
        line-join:round;
        line-cap: round;
        line-color: @street_limited;
        /*
        [zoom>=12] { line-width: 2; }
        [zoom>=15] { line-width: 2; }
        [zoom>=16] { line-width: 2.5; }
        [zoom>=17] { line-width: 3; }
        [zoom>=18] { line-width: 4; } 
        */
        line-width: linear([view::zoom], (12, 2), (15, 2), (16, 2.5), (17, 3), (18, 4));
      }
    }
    
    [class='major_rail'] {
      line-color: @major_rail;
      line-width: 0.7;
      [zoom>=15] {
        line-width: 1;
        h/line-width: 2.4;
        h/line-color: @major_rail;
        h/line-dasharray: 1,25;
      }
    }
    
    [class='siding_rail'] {
      line-color: @siding_rail;
      line-width: 0.7;
      [zoom>=15] {
        line-width: 1;
        h/line-width: 2.4;
        h/line-color: @siding_rail;   
        h/line-dasharray: 1, 25;
      }
    }
    
    [class='narrow_gauge'] {
      line-color: @siding_rail;
      line-width: 0.7;
      h/line-width: 2.5;
      h/line-color: @siding_rail;
      h/line-dasharray: 0.7, 25;
    }
    
    [class='tram'] {
      [zoom>=17] {
        line-color: @tram;
        line-width: 0.5;
        h/line-width: 1.2;
        h/line-color: @tram;
        h/line-dasharray: 0.5, 30;
      }
    }
  }
  ::arrow[oneway=1] {
    [class='motorway'],
    [class='motorway_link'],  
    [class='main'],
    [class='main_link'],
    [class='tertiary'],
    [class='tertiary_link'] { 
      [zoom>=16] {
        marker-opacity: 0.3;
        marker-placement: line;
        marker-file: url('icon/arrow-right.png');
        marker-spacing: 180;
        marker-width: 11;
        marker-height: 5;
      }
    }
    [class='street'],
    [class='street_limited'] {
      [zoom>=17] {
       marker-opacity: 0.3;
       marker-placement: line;
       marker-file: url('icon/arrow-right.png');
       marker-width: 9;
       marker-height: 4; 
       marker-spacing: 180;
      }
      [zoom>=18] {
       marker-width: 11;
       marker-height: 5; 
      }
    }
  } 
  ::arrow[oneway=-1] {
    [class='motorway'],
    [class='motorway_link'],  
    [class='main'],
    [class='main_link'],
    [class='tertiary'],
    [class='tertiary_link'] { 
      [zoom>=16] {
        marker-opacity: 0.3;
        marker-placement: line;
        marker-file: url('icon/arrow-left.png');
        marker-spacing: 180;
        marker-width: 11;
        marker-height: 5;
      }
    }
    [class='street'],
    [class='street_limited'] {
      [zoom>=17] {
       marker-opacity: 0.3;
       marker-placement: line;
       marker-file: url('icon/arrow-left.png');
       marker-width: 9;
       marker-height: 4; 
       marker-spacing: 180;
      }
      [zoom>=18] {
       marker-width: 11;
       marker-height: 5; 
      }
    }
  }
}

#route {
  ::case['mapnik::geometry_type'=2] {
    [class='hiking'][zoom>=10] {
      line-color: @hiking_route;
      line-dasharray: 2,1;
      /*
      [zoom>=16] { line-width: 1.2; }
      [zoom>=17] { line-width: 1.5; }
      */
      line-width: linear([view::zoom], (10, 1), (16, 1.2), (17, 1.5));
    }
    [class='ski'][zoom>=10] {
      line-color: @ski_route;
      line-dasharray: 2,1;
      /*
      [zoom>=16] { line-width: 1.2; }
      [zoom>=17] { line-width: 1.5; }
      */
      line-width: linear([view::zoom], (10, 1), (16, 1.2), (17, 1.5));
    }
    [class='ferry'][zoom>=8] {
      line-color: @ferry_route;
      line-dasharray: 7,2;
      /*
      [zoom>=13] { line-width: 1.2; }
      [zoom>=16] { line-width: 1.2; }
      [zoom>=17] { line-width: 1.5; }
      */
      line-width: linear([view::zoom], (8, 1), (13, 1.2), (16, 1.2), (17, 1.5));
    }
  }
}
