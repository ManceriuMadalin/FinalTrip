using Godot;
using System;
using System.Collections.Generic;

public partial class Global : Node
{
    public int frame = 0;
    public string selectedCountry = "";
    public double[] limit = new double[4];
    public int number_of_ghosts = 0;
    public int number_of_rival = 0;
    public int number_of_elfs = 0;

    public void UpdateLimits()
    {
        switch (selectedCountry)
        //stanga sus dreapta jos
        {
            case "Italy":
                limit = [0, 0, 1840, 960];
                break;
            case "Spain":
                limit = [-208, 0, 845, 865];
                break;
            case "USA":
                limit = [0, 0, 3000, 1900];
                break;
            case "Mexico":
                limit = [0, 0, 2800, 1160];
                break;
            case "Egypt": 
                limit = [0, 0, 1280, 1040];
                break;
            default:
                limit = [0, 0, 0, 0];
                break;
        }
    }

    public void UpdateLimitsGravity()
    {
        switch (selectedCountry)
        //stanga sus dreapta jos
        {
            case "Italy":
                limit = [0, -400, 1040, 160];
                break;
            case "Ireland":
                limit = [0, -560, 3800, 160];
                break;
            default:
                limit = [0, 0, 0, 0];
                break;
        }
    }

    public void add_frame()
    {
        if (frame == 9) frame = -1;
        frame++;
    }

    public void substract_frame()
    {
        if (frame == 0) frame = 10;
        frame--;
    }
}