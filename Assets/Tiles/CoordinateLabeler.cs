using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways]
[RequireComponent(typeof(TextMeshPro))]
public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.grey;
    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    Vector3 incrementalSnap;
    Waypoint waypoint;

    void Awake()
    {
        waypoint = GetComponentInParent<Waypoint>();

        label = GetComponent<TextMeshPro>();
        label.enabled = false;
        incrementalSnap = UnityEditor.EditorSnapSettings.move; // things associated with UnityEditor CANNOT be built into final project!!!

        SetLabelColor();
        DisplayCoordinates();
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
        }
        SetLabelColor();
        ToggleLabels(); // unavailable at Edit Mode???
    }

    void DisplayCoordinates()
    {
        // Debug.Log(incrementalSnap);
        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / incrementalSnap.x);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / incrementalSnap.z);
        label.text = $"{coordinates.x},{coordinates.y}";
    }

    void UpdateObjectName() {
        transform.parent.name = coordinates.ToString();
    }

    void SetLabelColor()
    {
        if (waypoint.IsPlaceable)
        {
            label.color = defaultColor;
        }
        else 
        {
            label.color = blockedColor;
        }
    }

    void ToggleLabels()
    {
        if (Input.GetKeyDown(KeyCode.C)) // C for Coordinate labels
        {
            label.enabled = !label.enabled;
        }
    }
}
