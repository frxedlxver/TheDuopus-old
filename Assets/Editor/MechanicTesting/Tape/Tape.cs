using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public enum PivotPoint
{
    LEFT, RIGHT, CENTER
}

[ExecuteAlways]
public class Tape : MonoBehaviour
{
    [Header("Don't edit these directly. public for debugging purposes. ")]
    public List<Transform> Nodes = new();
    public List<Rigidbody2D> NodeBodies = new();
    public List<HingeJoint2D> NodeHinges = new();
    public List<FixedJoint2D> NodeAffixionPoints = new();

    public GameObject NodeTemplate;

    [Header("Use these like buttons when changing the nodes")]
    public bool UpdateNodeList = false;
    public bool AdjustNodes = false;

    [Header("Tape properties")]
    public Rigidbody2D AdhesionTarget;
    public PivotPoint pivot = PivotPoint.CENTER;
    public float AdhesiveForce = 300f;

    [Header("Node transform properties")]
    public float NodeSpacing = 0.5f;
    public float NodeWidth = 1f;
    public float NodeHeight = 0.2f;

    [Header("Node RB properties")]
    public float NodeMass = 1;
    public float NodeGravityScale = 1;

    [Header("Node hinge angle properties")]
    public float NodeAngleLimitMin = -45;
    public float NodeAngleLimitMax = 45;

    private int NodeCount
    {
        get
        {
            return Nodes == null ? 0 : Nodes.Count;
        }
    }

    private float TapeWidth
    {
        get
        {
            return ((NodeWidth * NodeCount) + (NodeSpacing * (NodeCount - 3)));
        }
    }
    private float NodeTransformOffset 
    { 
        get
        {
            switch (pivot)
            {
                case PivotPoint.LEFT:
                    return 0;
                case PivotPoint.RIGHT:
                    return -TapeWidth; 
                case PivotPoint.CENTER:
                    return -TapeWidth / 2f;
                default: 
                    return 0;
            }        
        } 
    }

    private void OnEnable()
    {
        if (UpdateNodeList || AdjustNodes)
        {
            Debug.LogWarning("Warning: UpdateNodeList or AdjustNodes are enabled in the editor for " + this.name + ". This can cause unneccessary overhead and should be disabled when you are not actively editing the nodes.");
        }
    }

    private void Update()
    {
        if (UpdateNodeList)
        {
            PopulateNodeList();
        }

        if (AdjustNodes && Nodes.Count > 0)
        {
            for(int i = 0; i < Nodes.Count; i++)
            {
                Transform curNode = Nodes[i];
                if (curNode != null)
                {
                    UpdateNodeTransform(curNode, i);
                    UpdateNodeRb(NodeBodies[i]);
                    if (i < Nodes.Count - 1)
                    {
                        UpdateNodeHinge(NodeHinges[i], NodeBodies[i + 1]);
                    }
                    else
                    {
                        NodeHinges[i].enabled = false;
                    }
                }
            }
        }
    }

    private void UpdateNodeHinge(HingeJoint2D nodeHinge, Rigidbody2D connectedRB)
    {
        nodeHinge.enabled = true;
        nodeHinge.connectedBody = connectedRB;
        nodeHinge.autoConfigureConnectedAnchor = true;
        nodeHinge.autoConfigureConnectedAnchor = false;
        nodeHinge.useLimits = true;

        JointAngleLimits2D limits = nodeHinge.limits;
        limits.min = NodeAngleLimitMax;
        limits.max = NodeAngleLimitMin;
        nodeHinge.limits = limits;
    }

    private void UpdateNodeRb(Rigidbody2D nodeRB)
    {
        nodeRB.mass = NodeMass;
        nodeRB.gravityScale = NodeGravityScale;
    }

    private void UpdateNodeFixedJoint(FixedJoint2D nodeAffixionPoint)
    {
        nodeAffixionPoint.breakForce = AdhesiveForce;
        nodeAffixionPoint.connectedBody = AdhesionTarget;
        nodeAffixionPoint.autoConfigureConnectedAnchor = true;
        nodeAffixionPoint.autoConfigureConnectedAnchor = false;
    }

    private void UpdateNodeTransform(Transform node, int index)
    {
        node.localScale = new Vector2(NodeWidth, NodeHeight);
        node.localPosition = Vector2.right * (((NodeWidth + NodeSpacing) * index) + NodeTransformOffset);
    }

    private void PopulateNodeList()
    {
        if (Nodes.Count != this.transform.childCount)
        {
            Nodes = GetComponentsInChildren<Transform>().ToList();
            if (Nodes.Contains(this.transform))
            {
                Nodes.Remove(this.transform);
            }

            NodeBodies.Clear();
            NodeHinges.Clear();
            NodeAffixionPoints.Clear();

            foreach (Transform t in Nodes)
            {
                
                if (!t.TryGetComponent<HingeJoint2D>(out var curHinge))
                {
                    curHinge = t.AddComponent<HingeJoint2D>();
                }

                if (!t.TryGetComponent<Rigidbody2D>(out var curRB))
                {
                    curRB = t.AddComponent<Rigidbody2D>();
                }

                if (!t.TryGetComponent<FixedJoint2D>(out var curAffix))
                {
                    curAffix = t.AddComponent<FixedJoint2D>();
                }


                NodeBodies.Add(t.GetComponent<Rigidbody2D>());
                NodeHinges.Add(t.GetComponent<HingeJoint2D>());
                NodeAffixionPoints.Add(t.GetComponent<FixedJoint2D>());
            }
        }
    }
}
