using Godot;
using System;
using System.Collections.Generic;

public partial class LevelsMenu : Node2D
{
  private Global global;
  private Dictionary<string, Node2D> locationMarkers = new Dictionary<string, Node2D>();
  private PackedScene locationMarkerScene;

  private Dictionary<string, Dictionary<int, Vector2>> markerPositions = new Dictionary<string, Dictionary<int, Vector2>>();

  private Dictionary<string, Dictionary<int, bool>> markerVisibility = new Dictionary<string, Dictionary<int, bool>>();

  public override void _Ready()
  {
    GetNode<Button>("%Left_Btn").Pressed += _Left;
    GetNode<Button>("%Right_Btn").Pressed += _Right;
    global = GetNode<Global>("/root/Global");

    locationMarkerScene = GD.Load<PackedScene>("res://Scenes/location_marker.tscn");

    InitializeMarkerData();
    CreateMarkers();
    UpdateMarkers();
  }

  public override void _Process(double delta)
  {
    UpdateMarkers();
  }

  private void InitializeMarkerData()
  {
    string[] countries = { "Italy", "Spain", "USA", "Australia", "Mexico", "Ireland", "Egypt" };

    foreach (string country in countries)
    {
      markerPositions[country] = new Dictionary<int, Vector2>();
      markerVisibility[country] = new Dictionary<int, bool>();

      for (int i = 0; i < 10; i++)
      {
        markerPositions[country][i] = Vector2.Zero; 
        markerVisibility[country][i] = true; 
      }
    }
    
    markerPositions["Italy"][0] = new Vector2(276, 207);
    markerPositions["Italy"][1] = new Vector2(426, 207);
    markerPositions["Italy"][2] = new Vector2(576, 207);
    markerPositions["Italy"][3] = new Vector2(726, 207);
    markerPositions["Italy"][4] = new Vector2(876, 207);
    markerVisibility["Italy"][5] = false;
    markerVisibility["Italy"][6] = false;
    markerVisibility["Italy"][7] = false;
    markerVisibility["Italy"][8] = false;
    markerVisibility["Italy"][9] = false;

    markerPositions["Spain"][0] = new Vector2(202, 198);
    markerPositions["Spain"][1] = new Vector2(352, 198);
    markerPositions["Spain"][2] = new Vector2(502, 198);
    markerPositions["Spain"][3] = new Vector2(652, 198);
    markerPositions["Spain"][4] = new Vector2(802, 198);
    markerPositions["Spain"][5] = new Vector2(952, 198);
    markerVisibility["Spain"][6] = false;
    markerVisibility["Spain"][7] = false;
    markerVisibility["Spain"][8] = false;
    markerVisibility["Spain"][9] = false;

    markerVisibility["USA"][0] = false;
    markerVisibility["USA"][1] = false;
    markerVisibility["USA"][2] = false;
    markerPositions["USA"][3] = new Vector2(250, 155);
    markerPositions["USA"][4] = new Vector2(400, 155);
    markerPositions["USA"][5] = new Vector2(550, 155);
    markerPositions["USA"][6] = new Vector2(700, 155);
    markerPositions["USA"][7] = new Vector2(850, 155);
    markerVisibility["USA"][8] = false;
    markerVisibility["USA"][9] = false;

    //markerPositions["Australia"][0] = new Vector2(735, 570);
    //markerPositions["Australia"][1] = new Vector2(885, 570);
    //markerVisibility["Australia"][2] = false;
    //markerVisibility["Australia"][3] = false;
    //markerVisibility["Australia"][4] = false;
    //markerVisibility["Australia"][5] = false;
    //markerVisibility["Australia"][6] = false;
    //markerVisibility["Australia"][7] = false;
    //markerPositions["Australia"][8] = new Vector2(310, 570);
    //markerPositions["Australia"][9] = new Vector2(460, 570);

    markerVisibility["Mexico"][0] = false;
    markerVisibility["Mexico"][1] = false;
    markerVisibility["Mexico"][2] = false;
    markerPositions["Mexico"][3] = new Vector2(240, 235);
    markerPositions["Mexico"][4] = new Vector2(390, 235);
    markerPositions["Mexico"][5] = new Vector2(540, 235);
    markerPositions["Mexico"][6] = new Vector2(690, 235);
    markerPositions["Mexico"][7] = new Vector2(840, 235);
    markerVisibility["Mexico"][8] = false;
    markerVisibility["Mexico"][9] = false;

    markerVisibility["Ireland"][0] = false;
    markerVisibility["Ireland"][1] = false;
    markerPositions["Ireland"][2] = new Vector2(355, 80);
    markerPositions["Ireland"][3] = new Vector2(505, 80);
    markerPositions["Ireland"][4] = new Vector2(650, 80);
    markerPositions["Ireland"][5] = new Vector2(805, 80);
    markerVisibility["Ireland"][6] = false;
    markerVisibility["Ireland"][7] = false;
    markerVisibility["Ireland"][8] = false;
    markerVisibility["Ireland"][9] = false;

    //markerPositions["Egypt"][0] = new Vector2(290, 345);
    //markerPositions["Egypt"][1] = new Vector2(440, 345);
    //markerPositions["Egypt"][2] = new Vector2(590, 345);
    //markerPositions["Egypt"][3] = new Vector2(740, 345);
    //markerPositions["Egypt"][4] = new Vector2(890, 345);
    //markerVisibility["Egypt"][5] = false;
    //markerVisibility["Egypt"][6] = false;
    //markerVisibility["Egypt"][7] = false;
    //markerVisibility["Egypt"][8] = false;
    //markerVisibility["Egypt"][9] = false;
  }

  private void CreateMarkers()
{
    string[] countries = { "Italy", "Spain", "USA", "Mexico", "Ireland" };

    foreach (string country in countries)
    {
        Node2D marker = locationMarkerScene.Instantiate<Node2D>();
        marker.Name = country;
        AddChild(marker);
        locationMarkers[country] = marker;

        Label countryLabel = new Label();
        countryLabel.Text = country;
        countryLabel.Position = new Vector2(-20, 20); 
        marker.AddChild(countryLabel);
        
        Button countryButton = marker.GetNode<Button>("Button");
        countryButton.Pressed += () => OnCountrySelected(country);
    }
}

private void OnCountrySelected(string country)
{
    global.selectedCountry = country;
    global.UpdateLimits();
    
    string scenePath = $"res://Scenes/{country}/{country.ToLower()}_car_trip.tscn";
    
    if (ResourceLoader.Exists(scenePath))
    {
        GetTree().ChangeSceneToFile(scenePath);
    }
}

 private void UpdateMarkers()
{
    int currentFrame = global.frame;

    foreach (string country in locationMarkers.Keys)
    {
        Node2D marker = locationMarkers[country];
        bool shouldBeVisible = markerVisibility[country][currentFrame];

        if (shouldBeVisible)
        {
            marker.Visible = true;
            marker.Position = markerPositions[country][currentFrame];
            foreach (Node child in marker.GetChildren())
            {
                if (child is CanvasItem canvasItem)
                {
                    canvasItem.Visible = true;
                }
            }
        }
        else
        {
            marker.Visible = false;
            marker.Position = new Vector2(-1000, -1000);
            foreach (Node child in marker.GetChildren())
            {
                if (child is CanvasItem canvasItem)
                {
                    canvasItem.Visible = false;
                }
            }
        }
    }
}
  public void _Left()
  {
    global.substract_frame();
  }

  public void _Right()
  {
    global.add_frame();
  }
}