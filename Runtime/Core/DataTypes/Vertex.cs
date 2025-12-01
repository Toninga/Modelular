using JetBrains.Annotations;
using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Modelular.Data
{
    [System.Serializable]
    public struct Vertex
    {
        public float x { get => position.x; set => position.x = value; }
        public float y { get => position.y; set => position.y = value; }
        public float z { get => position.z; set => position.z = value; }
        public Vector3 position;
        public Vector3 normal;
        public Color color;
        public Vector2 UV0;
        public Vector2 UV1;
        public Vector2 UV2;
        public Vector2 UV3;
        public short submesh;
        public string SelectionGroup;

        /// <summary>
        /// Returns a new vertex of a sphere
        /// </summary>
        /// <param name="radius">Radius of the sphere</param>
        /// <param name="latitude">How high the vertex is, from -PI/2 to PI/2</param>
        /// <param name="longitude">Where around the trigonometric circle the vertex is, from 0 to 2PI</param>
        /// <returns></returns>
        public static Vertex VertexFromPolarCoord(float radius, float latitude, float longitude)
        {
            Vector3 pos = new Vector3(
                Mathf.Cos(longitude) * Mathf.Cos(latitude) * radius,
                Mathf.Sin(latitude) * radius,
                Mathf.Sin(longitude) * Mathf.Cos(latitude) * radius
                );
            Vector3 normal = pos.normalized;
            Vector2 UV = new Vector2(latitude * 2 / Mathf.PI + 1, longitude / 2 / Mathf.PI);
            return new Vertex(pos, normal);
        }
        public Vertex Flipped() => new Vertex(position, -normal, color, submesh, UV0, UV1, UV2, UV3, SelectionGroup);

        public override bool Equals(object obj)
        {
            return obj is Vertex vertex &&
                   position.Equals(vertex.position) &&
                   normal.Equals(vertex.normal) &&
                   color.Equals(vertex.color) &&
                   UV0.Equals(vertex.UV0) &&
                   UV1.Equals(vertex.UV1) &&
                   UV2.Equals(vertex.UV2) &&
                   UV3.Equals(vertex.UV3) &&
                   submesh == vertex.submesh &&
                   SelectionGroup.Equals(vertex.SelectionGroup);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position, normal, color, UV0, UV1, UV2, UV3, submesh);
        }

        public Vertex(
            Vector3 position,
            Vector3 normal = default,
            Color color = default,
            short submesh = default,
            Vector2 UV0 = default,
            Vector2 UV1 = default,
            Vector2 UV2 = default,
            Vector2 UV3 = default,
            string selectionGroup = default)
        {
            this.position = position;
            this.normal = normal;
            this.color = color;
            this.UV0 = UV0;
            this.UV1 = UV1;
            this.UV2 = UV2;
            this.UV3 = UV3;
            this.submesh = submesh;
            this.SelectionGroup = selectionGroup;
        }

        public Vertex(Vertex reference)
        {
            this.position = reference.position;
            this.normal = reference.normal;
            this.color = reference.color;
            this.UV0 = reference.UV0;
            this.UV1 = reference.UV1;
            this.UV2 = reference.UV2;
            this.UV3 = reference.UV3;
            this.submesh = reference.submesh;
            this.SelectionGroup = reference.SelectionGroup;
        }

        public Vertex(Vertex reference,
            Vector3 overridePosition = default,
            Vector3 overrideNormal = default,
            Color overrideColor = default,
            short overrideSubmesh = default,
            Vector2 overrideUV0 = default,
            Vector2 overrideUV1 = default,
            Vector2 overrideUV2 = default,
            Vector2 overrideUV3 = default,
            string overrideSelectionGroup = default)
        {
            this.position = overridePosition == default ? reference.position : overridePosition ;
            this.normal = overrideNormal == default ? reference.normal : overrideNormal ;
            this.color = overrideColor == default ? reference.color : overrideColor;
            this.UV0 = overrideUV0 == default ? reference.UV0 : overrideUV0;
            this.UV1 = overrideUV1 == default ? reference.UV1 : overrideUV1 ;
            this.UV2 = overrideUV2 == default ? reference.UV2 : overrideUV2;
            this.UV3 = overrideUV3 == default ? reference.UV3 : overrideUV3;
            this.submesh = overrideSubmesh == default ? reference.submesh : overrideSubmesh;
            this.SelectionGroup = string.IsNullOrEmpty(overrideSelectionGroup) ? reference.SelectionGroup : overrideSelectionGroup;
        }

        public static bool operator ==(Vertex l, Vertex r) => 
            (l.position == r.position) && 
            (l.normal == r.normal) && 
            (l.color == r.color) && 
            (l.UV0 == r.UV0) && 
            (l.UV1 == r.UV1) && 
            (l.UV2 == r.UV2) && 
            (l.UV3 == r.UV3) && 
            (l.submesh == r.submesh) &&
            (l.SelectionGroup == r.SelectionGroup);

        public static implicit operator string(Vertex v)
        {
            string result = "[Vertex] ";
            result += "Position : " + v.position.ToString();
            result += "\nNormal : " + v.normal.ToString();
            result += "\nColor : " + v.color.ToString();
            result += "\nUV0 : " + v.UV0.ToString();
            result += "\nSelection group : " + v.SelectionGroup;
            return result;
        }
        public override string ToString()
        {
            return (string)this;
        }
        public static bool operator !=(Vertex l, Vertex r) => !(l == r);
        public static Vertex operator *(Matrix4x4 m, Vertex v)
        {
            return new Vertex(v, m * v.position, m * v.normal);
        }

        public static Vertex operator *(Quaternion q, Vertex v)
        {
            return new Vertex(v, q * v.position, q * v.normal);
        }

        public static Vertex operator +(Vertex v, Vector3 o)
        {
            return new Vertex(v, v.position + o);
        }
        public static Vertex operator +(Vector3 o, Vertex v) => v + o;
        public static Vertex operator -(Vertex v, Vector3 o) => v + (-o);
        public static Vertex operator -(Vector3 o, Vertex v) => v - o;

    }

}