using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TileManager2 : MonoBehaviour {

	public class Tile2
	{
		public int x { get; set; }
		public int y { get; set; }
		public bool occupied {get; set;}
		public int adjacentCount {get; set;}
		public bool isIntersection {get; set;}
		
		public Tile2 left,right,up,down;
		
		public Tile2(int x_in, int y_in)
		{
			x = x_in; y = y_in;
			occupied = false;
			left = right = up = down = null;
		}


	};
	
	public List<Tile2> tiles = new List<Tile2>();
	
	void Start () 
	{
        ReadTiles();

	}

	void Update () 
	{

	}
	
    // paredes estao hardcoded com 1 e 0. 1 é free e o 0 é uma parede
    void ReadTiles()
    {
        string data = @"0000000000000000000000000000
0111111111111001111111111110
0100001000001001000001000010
0100001000001001000001000010
0100001000001001000001000010
0111111111111111111111111110
0100001001000000001001000010
0100001001000000001001000010
0111111001111001111001111110
0000001000001001000001000000
0000001000001001000001000000
0000001001111111111001000000
0000001001000000001001000000
0000001001000000001001000000
0000001111000000001111000000
0000001001000000001001000000
0000001001000000001001000000
0000001001111111111001000000
0000001001000000001001000000
0000001001000000001001000000
0111111111111001111111111110
0100001000001001000001000010
0100001000001001000001000010
0111001111111001111111001110
0001001001000000001001001000
0001001001000000001001001000
0111111001111001111001111110
0100000000001001000000000010
0100000000001001000000000010
0111111111111111111111111110
0000000000000000000000000000";

        int X = 1, Y = 31;
        using (StringReader reader = new StringReader(data))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {

                X = 1; // para cada linha
                for (int i = 0; i < line.Length; ++i)
                {
                    Tile2 newTile = new Tile2(X, Y);

                    // se a linha escolhida no momento for valida
                    if (line[i] == '1')
                    {
                        // procurar por vizinhos na direita ou esquerda
                        if (i != 0 && line[i - 1] == '1')
                        {
                            // atribuir cada quadrado ao lado correspondente do outro quadrado
                            newTile.left = tiles[tiles.Count - 1];
                            tiles[tiles.Count - 1].right = newTile;

                            // ajudar a contagem de quadrados adjacentes, para cada quadrado
                            newTile.adjacentCount++;
                            tiles[tiles.Count - 1].adjacentCount++;
                        }
                    }

                    // se o quadrado estiver ocupado
                    else newTile.occupied = true;

                    // procurar por vizinhos em cima ou em baixo, a começar pela segunda linha (y<30)
                    int upNeighbor = tiles.Count - line.Length; // up neighbor index
                    if (Y < 30 && !newTile.occupied && !tiles[upNeighbor].occupied)
                    {
                        tiles[upNeighbor].down = newTile;
                        newTile.up = tiles[upNeighbor];

                        // ajustar a contagem de quadrados para cada um
                        newTile.adjacentCount++;
                        tiles[upNeighbor].adjacentCount++;
                    }

                    tiles.Add(newTile);
                    X++;
                }

                Y--;
            }
        }

        // depois de ler todos os quadrados hardcoded, determinar quais os que correspondem a inerseçoes
        foreach (Tile2 tile in tiles)
        {
            if (tile.adjacentCount > 2)
                tile.isIntersection = true;
        }

    }

    // escrever linhas entre quadrados vizinhos
	void DrawNeighbors()
	{
		foreach(Tile2 tile in tiles)
		{
			Vector3 pos = new Vector3(tile.x, tile.y, 0);
			Vector3 up = new Vector3(tile.x+0.1f, tile.y+1, 0);
			Vector3 down = new Vector3(tile.x-0.1f, tile.y-1, 0);
			Vector3 left = new Vector3(tile.x-1, tile.y+0.1f, 0);
			Vector3 right = new Vector3(tile.x+1, tile.y-0.1f, 0);
			
			if(tile.up != null)		Debug.DrawLine(pos, up);
			if(tile.down != null)	Debug.DrawLine(pos, down);
			if(tile.left != null)	Debug.DrawLine(pos, left);
			if(tile.right != null)	Debug.DrawLine(pos, right);
		}
		
	}


    // devolve o indice da lista de quadrados de uma coordenada de um quadrado
	public int Index(int X, int Y)
	{
        //se o indice for valido
		if(X>=1 && X<=28 && Y<=31 && Y>=1)
			return (31-Y)*28 + X-1;

        // else, se o presente indice nao estiver disponivel, retorna o indice mais perto que esteja disponivel
	    if(X<1)		X = 1;
	    if(X>28) 	X = 28;
	    if(Y<1)		Y = 1;
	    if(Y>31)	Y = 31;

	    return (31-Y)*28 + X-1;
	}
	
	public int Index(Tile2 tile)
	{
		return (31-tile.y)*28 + tile.x-1;
	}

    // distancia entre dois quadrados
	public float distance(Tile2 tile1, Tile2 tile2)
	{
		return Mathf.Sqrt( Mathf.Pow(tile1.x - tile2.x, 2) + Mathf.Pow(tile1.y - tile2.y, 2));
	}
}
