// See https://aka.ms/new-console-template for more information



int[][] flights1 = [[0, 1, 100], [1, 2, 100], [2, 0, 100], [1,3,600], [2, 3, 200]];
int[][] flights2 = [[0, 1, 100], [1, 2, 100], [0, 2, 500]];
int[][] flights3 = [[0, 1, 100], [1, 2, 100], [0, 2, 500]];
int[][] flights4 =
[
    [3, 4, 4], [2, 5, 6], [4, 7, 10], [9, 6, 5], [7, 4, 4], [6, 2, 10], [6, 8, 6], [7, 9, 4], [1, 5, 4], [1, 0, 4],
    [9, 7, 3], [7, 0, 5], [6, 5, 8], [1, 7, 6], [4, 0, 9], [5, 9, 1], [8, 7, 3], [1, 2, 6], [4, 1, 5], [5, 2, 4],
    [1, 9, 1], [7, 8, 10], [0, 4, 2], [7, 2, 8]
];
int[][] flights5 = [[2, 3, 200],  [1, 2, 100], [2, 0, 100], [1,3,600], [0, 1, 100]];
var sln = new Solution();

int[][] fl6 = [[0,1,20],[1,2,20],[2,3,20],[3,4,20],[4,5,20],[5,6,20],[6,7,20],[7,8,20],[8,9,20],[0,2,9999],[2,4,9998],[4,7,9997]];
var chp = sln.FindCheapestPrice(4,flights1, 0,3,1);
var chp1 = sln.FindCheapestPrice(3, flights2, 0, 2, 1);
var chp2 = sln.FindCheapestPrice(3, flights3, 0, 2, 0);
var chp3 = sln.FindCheapestPrice(10, flights4, 6, 0, 7);
var chp4 = sln.FindCheapestPrice(10,fl6, 0,9,4);
var a = 3;




public class Sl2
{
    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k) {
        int[] distances = new int[n];
        Array.Fill(distances, int.MaxValue);
        distances[src] = 0;

        for (int i = 0; i <= k; ++i) {
            int[] temp = (int[])distances.Clone();
            
            foreach (var flight in flights) {
                int from = flight[0], to = flight[1], cost = flight[2];
                if (distances[from] != int.MaxValue && distances[from] + cost < temp[to]) {
                    temp[to] = distances[from] + cost;
                }
            }

            distances = temp;
        }

        return distances[dst] == int.MaxValue ? -1 : distances[dst];
    }
}