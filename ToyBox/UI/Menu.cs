using Microsoft.Xna.Framework;
using Myra.Graphics2D.UI;
using System.Collections.Generic;

public class Menu
{
    // Myra UI Desktop for rendering UI elements
    private Desktop _desktop;
    
    // Variable to store the currently selected gate type
    private string selectedGateType;

    // Method to initialize the menu and components
    public void InitializeMenu()
    {
        // Create the desktop (root for Myra UI elements)
        _desktop = new Desktop();

        // Create a panel to hold the menu items
        var panel = new Panel
        {
            Width = 400,
            Height = 300
        };

        // Add a label for the title
        var titleLabel = new Label
        {
            Text = "Component Selection Menu",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Top
        };
        panel.Widgets.Add(titleLabel);

        // Add buttons for each gate type (AND, OR, NOT, XOR)
        AddGateButton(panel, "AND Gate", "AND");
        AddGateButton(panel, "OR Gate", "OR");
        AddGateButton(panel, "NOT Gate", "NOT");
        AddGateButton(panel, "XOR Gate", "XOR");

        // Add a label to show the currently selected gate type
        var selectedGateLabel = new Label
        {
            Text = "Selected Gate: None",
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Bottom
        };
        panel.Widgets.Add(selectedGateLabel);

        // Update the selected gate label when a gate is chosen
        void SetSelectedGateType(string gateType)
        {
            selectedGateType = gateType;
            selectedGateLabel.Text = $"Selected Gate: {gateType}";
        }

        // Set the panel as the root widget of the desktop
        _desktop.Root = panel;
    }

    // Helper method to add a gate selection button
// Helper method to add a gate selection button with a label
private void AddGateButton(Panel panel, string buttonText, string gateType)
{
    var button = new Button
    {
        Width = 200,
        Height = 50,
        HorizontalAlignment = HorizontalAlignment.Center
    };

    // Create a label and set it as the button's content
    var label = new Label
    {
        Text = buttonText,
        HorizontalAlignment = HorizontalAlignment.Center,
        VerticalAlignment = VerticalAlignment.Center
    };

    button.Content = label;

    // Event handler for when the button is clicked
    button.Click += (sender, args) =>
    {
        // Call a method to handle setting the selected gate type
        SetSelectedGateType(gateType);
    };

    // Add the button to the panel
    panel.Widgets.Add(button);
}


    // Method to render the UI and any dragged component
    public void Draw()
    {
        // Render the Myra UI
        _desktop.Render();
    }

    // Update the selected gate label text when a gate is selected
    private void SetSelectedGateType(string gateType)
    {
        selectedGateType = gateType;
    }
}
