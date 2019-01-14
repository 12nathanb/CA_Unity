using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MeshGenerator : MonoBehaviour
{
    public MeshFilter walls;
    public SquareGrid squareGrid;
    List<Vector3> vertices;
    List<int> triangles;

    Dictionary<int, List<Triangle>> triDic = new Dictionary<int, List<Triangle>>();
    List<List<int>> outline = new List<List<int>>();
    HashSet<int> verticesAlreadyChecked = new HashSet<int>();

    public void GenerateMesh(int[,] map, float squareSize)
    {
        outline.Clear();
        verticesAlreadyChecked.Clear();
        triDic.Clear();

        squareGrid = new SquareGrid(map, squareSize);

        vertices = new List<Vector3>();
        triangles = new List<int>();

        for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
        {
            for (int y = 0; y < squareGrid.squares.GetLength(1); y++)
            {
                TriangulateSquare(squareGrid.squares[x, y]);
            }
        }

        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
       // walls = this.GetComponent<MeshFilter>();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        ExtrudeMeshForWalls();
        
    }

    void ExtrudeMeshForWalls()
    {
        MeshOutline();

        List<int> triWalls = new List<int>();

        Mesh meshWall = new Mesh();

        float heightOfWall = 8;

        List<Vector3> verticeOfWall = new List<Vector3>();



        foreach (List<int> outlines in outline)
        {

            for (int i = 0; i < outlines.Count - 1; i++)
            {

                int startVertex = verticeOfWall.Count;

                verticeOfWall.Add(vertices[outlines[i]]);

                verticeOfWall.Add(vertices[outlines[i + 1]]);

                verticeOfWall.Add(vertices[outlines[i]] - Vector3.up * heightOfWall);

                verticeOfWall.Add(vertices[outlines[i + 1]] - Vector3.up * heightOfWall);



                triWalls.Add(startVertex + 0);

                triWalls.Add(startVertex + 2);

                triWalls.Add(startVertex + 3);



                triWalls.Add(startVertex + 3);

                triWalls.Add(startVertex + 1);

                triWalls.Add(startVertex + 0);

            }

        }

        meshWall.vertices = verticeOfWall.ToArray();

        meshWall.triangles = triWalls.ToArray();

        walls.mesh = meshWall;
    }

    struct Triangle
    {

        public int vertexOne, vertexTwo, vertexThree;

        int[] v; // for vertices



        public Triangle(int _vertexOne, int _vertexTwo, int _vertexThree)
        {

            vertexOne = _vertexOne;

            vertexTwo = _vertexTwo;

            vertexThree = _vertexThree;



            v = new int[3];

            v[0] = _vertexOne;

            v[1] = _vertexTwo;

            v[2] = _vertexThree;

        }



        public int this[int i]
        {

            get
            {

                return v[i];

            }

        }



        public bool Contains(int i)
        {

            return i == vertexOne || i == vertexTwo || i == vertexThree;

        }

    }

    void TriangulateSquare(Square _square)
    {
        switch (_square.config)
        {

            case 0:

                break; // in this case there is no mesh



            // 1 pseudo-node

            case 1:

                CreateMeshFromNodes(_square.west, _square.south, _square.southwestCorner);

                break;

            case 2:

                CreateMeshFromNodes(_square.southeastCorner, _square.south, _square.east);

                break;

            case 4:

                CreateMeshFromNodes(_square.northeastCorner, _square.east, _square.north);

                break;

            case 8:

                CreateMeshFromNodes(_square.northwestCorner, _square.north, _square.west);

                break;



            // 2 pseudo=nodes

            case 3:

                CreateMeshFromNodes(_square.east, _square.southeastCorner, _square.southwestCorner, _square.west);

                break;

            case 6:

                CreateMeshFromNodes(_square.north, _square.northeastCorner, _square.southeastCorner, _square.south);

                break;

            case 9:

                CreateMeshFromNodes(_square.northwestCorner, _square.north, _square.south, _square.southwestCorner);

                break;

            case 12:

                CreateMeshFromNodes(_square.northwestCorner, _square.northeastCorner, _square.east, _square.west);

                break;

            case 5: // diagonal

                CreateMeshFromNodes(_square.north, _square.northeastCorner, _square.east, _square.south, _square.southwestCorner, _square.west);

                break;

            case 10: // diagonal

                CreateMeshFromNodes(_square.northwestCorner, _square.north, _square.east, _square.southeastCorner, _square.south, _square.west);

                break;



            // 3 pseudo=node

            case 7:

                CreateMeshFromNodes(_square.north, _square.northeastCorner, _square.southeastCorner, _square.southwestCorner, _square.west);

                break;

            case 11:

                CreateMeshFromNodes(_square.northwestCorner, _square.north, _square.east, _square.southeastCorner, _square.southwestCorner);

                break;

            case 13:

                CreateMeshFromNodes(_square.northwestCorner, _square.northeastCorner, _square.east, _square.south, _square.southwestCorner);

                break;

            case 14:

                CreateMeshFromNodes(_square.northwestCorner, _square.northeastCorner, _square.southeastCorner, _square.south, _square.west);

                break;



            // 4 pseudo-nodes

            case 15:

                CreateMeshFromNodes(_square.northwestCorner, _square.northeastCorner, _square.southeastCorner, _square.southwestCorner);

                verticesAlreadyChecked.Add(_square.northwestCorner.vertexIndex);

                verticesAlreadyChecked.Add(_square.northeastCorner.vertexIndex);

                verticesAlreadyChecked.Add(_square.southeastCorner.vertexIndex);

                verticesAlreadyChecked.Add(_square.southwestCorner.vertexIndex);

                break;

        }
    }

    void CreateMeshFromNodes(params Node[] points)
    {
        AssignVertices(points);

        if (points.Length >= 3)
            CreateTriangle(points[0], points[1], points[2]);
        if (points.Length >= 4)
            CreateTriangle(points[0], points[2], points[3]);
        if (points.Length >= 5)
            CreateTriangle(points[0], points[3], points[4]);
        if (points.Length >= 6)
            CreateTriangle(points[0], points[4], points[5]);
    }

    void AssignVertices(Node[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            if (points[i].vertexIndex == -1)
            {
                points[i].vertexIndex = vertices.Count;
                vertices.Add(points[i].position);
            }
        }
    }

    void CreateTriangle(Node a, Node b, Node c)
    {
        triangles.Add(a.vertexIndex);
        triangles.Add(b.vertexIndex);
        triangles.Add(c.vertexIndex);

        Triangle tri = new Triangle(a.vertexIndex, b.vertexIndex, c.vertexIndex);

        AddTriDictionary(tri.vertexOne, tri);

        AddTriDictionary(tri.vertexTwo, tri);

        AddTriDictionary(tri.vertexThree, tri);
    }

    void AddTriDictionary(int vertexKey, Triangle tri)
    {

        if (triDic.ContainsKey(vertexKey))
        {

            triDic[vertexKey].Add(tri);

        }
        else
        {

            List<Triangle> triList = new List<Triangle>();

            triList.Add(tri);

            triDic.Add(vertexKey, triList);

        }

    }

    void MeshOutline()
    {

        for (int vertexIndex = 0; vertexIndex < vertices.Count; vertexIndex++)
        {

            if (!verticesAlreadyChecked.Contains(vertexIndex))
            {

                int newOutlineVertex = GetConnectedOutlineVertex(vertexIndex);

                if (newOutlineVertex != -1)
                {

                    verticesAlreadyChecked.Add(vertexIndex);

                    List<int> newOutline = new List<int>();

                    outline.Add(newOutline);

                    FollowOutline(newOutlineVertex, outline.Count - 1);

                    outline[outline.Count - 1].Add(vertexIndex);

                }

            }

        }

    }

    void FollowOutline(int _vertexIndex, int _outlineIndex)
    {

        outline[_outlineIndex].Add(_vertexIndex);

        verticesAlreadyChecked.Add(_vertexIndex);

        int nextVertexIndex = GetConnectedOutlineVertex(_vertexIndex);

        if (nextVertexIndex != -1)
        {

            FollowOutline(nextVertexIndex, _outlineIndex);

        }

    }

    bool OutlineEdge(int vertexOne, int vertexTwo)
    {

        int counter = 0; // to track the number of triangles shared by the current vertex

        List<Triangle> trianglesThatContainsVertexOne = triDic[vertexOne];



        for (int i = 0; i < trianglesThatContainsVertexOne.Count; i++)
        {// count thats the number of values in the list

            if (trianglesThatContainsVertexOne[i].Contains(vertexTwo))
            {

                counter++;

                if (counter > 1)
                {

                    break;

                }

            }

        }

        return counter == 1;

    }



    // To get a list of all triangles that contain the vertexOne parameter

    int GetConnectedOutlineVertex(int _vertexOne)
    {

        List<Triangle> triThatContainsVertexOne = triDic[_vertexOne];



        for (int i = 0; i < triThatContainsVertexOne.Count; i++)
        {

            Triangle tri = triThatContainsVertexOne[i];



            for (int j = 0; j < 3; j++)
            {

                int vertex = tri[j];

                if (vertex != _vertexOne && !verticesAlreadyChecked.Contains(vertex))
                {

                    if (OutlineEdge(_vertexOne, vertex))
                    {

                        return vertex;

                    }

                }

            }

        }

        return -1;

    }

    public class SquareGrid
    {
        public Square[,] squares;

        public SquareGrid(int[,] map, float squareSize)
        {
            int nodeCountX = map.GetLength(0);
            int nodeCountY = map.GetLength(1);
            float levelWidth = nodeCountX * squareSize;

            float levelHeight = nodeCountY * squareSize;


            ControlNode[,] controlNodes = new ControlNode[nodeCountX, nodeCountY];

            for (int x = 0; x < nodeCountX; x++)
            {
                for (int y = 0; y < nodeCountY; y++)
                {
                    Vector3 position = new Vector3(-levelWidth / 2 + x * squareSize + squareSize / 2, 0, -levelHeight / 2 + y * squareSize + squareSize / 2);

                    controlNodes[x, y] = new ControlNode(position, map[x, y] == 0, squareSize);
                }
            }

            squares = new Square[nodeCountX - 1, nodeCountY - 1];
            for (int x = 0; x < nodeCountX - 1; x++)
            {
                for (int y = 0; y < nodeCountY - 1; y++)
                {
                    squares[x, y] = new Square(controlNodes[x, y + 1], controlNodes[x + 1, y + 1], controlNodes[x + 1, y], controlNodes[x, y]);
                }
            }

        }
    }

    public class Square
    {

        public ControlNode northwestCorner, northeastCorner, southeastCorner, southwestCorner;
		public Node  north, east, south, west;
		public int config;



		public Square(ControlNode _northwestCorner, ControlNode _northeastCorner,
            ControlNode _southeastCorner, ControlNode _southwestCorner){

			northwestCorner = _northwestCorner;
			northeastCorner = _northeastCorner;
			southeastCorner = _southeastCorner;
			southwestCorner = _southwestCorner;



			north = northwestCorner.right;
			east = southeastCorner.above;
			south = southwestCorner.right;
			west = southwestCorner.above;


			if(northwestCorner.active)
            {
				config += 8;
			}

			if(northeastCorner.active)
            {

				config += 4;

			}

			if(southeastCorner.active)
            {

				config += 2;

			}

			if(southwestCorner.active)
            {

				config += 1;

			}

		}
    }

    public class Node
    {
        public Vector3 position;
        public int vertexIndex = -1;

        public Node(Vector3 _pos)
        {
            position = _pos;
        }
    }

    public class ControlNode : Node
    {

        public bool active;
        public Node above, right;

        public ControlNode(Vector3 _pos, bool _active, float squareSize) : base(_pos)
        {
            active = _active;
            above = new Node(position + Vector3.forward * squareSize / 2f);
            right = new Node(position + Vector3.right * squareSize / 2f);
        }
    }

    void OnDrawGizmos()
    {
        if (squareGrid != null)
        {

            for (int x = 0; x < squareGrid.squares.GetLength(0); x++)
            {

                for (int z = 0; z < squareGrid.squares.GetLength(1); z++)
                {

                    Gizmos.color = (squareGrid.squares[x, z].northwestCorner.active) ? Color.blue : Color.yellow;

                    Gizmos.DrawCube(squareGrid.squares[x, z].northwestCorner.position, Vector3.one * .4f);



                    Gizmos.color = (squareGrid.squares[x, z].northeastCorner.active) ? Color.blue : Color.yellow;

                    Gizmos.DrawCube(squareGrid.squares[x, z].northeastCorner.position, Vector3.one * .4f);



                    Gizmos.color = (squareGrid.squares[x, z].southeastCorner.active) ? Color.blue : Color.yellow;

                    Gizmos.DrawCube(squareGrid.squares[x, z].southeastCorner.position, Vector3.one * .4f);



                    Gizmos.color = (squareGrid.squares[x, z].southwestCorner.active) ? Color.blue : Color.yellow;

                    Gizmos.DrawCube(squareGrid.squares[x, z].southwestCorner.position, Vector3.one * .4f);



                    Gizmos.color = Color.black;

                    Gizmos.DrawCube(squareGrid.squares[x, z].north.position, Vector3.one * .15f);

                    Gizmos.DrawCube(squareGrid.squares[x, z].east.position, Vector3.one * .15f);

                    Gizmos.DrawCube(squareGrid.squares[x, z].south.position, Vector3.one * .15f);

                    Gizmos.DrawCube(squareGrid.squares[x, z].west.position, Vector3.one * .15f);

                }

            }

        }

    }



}

