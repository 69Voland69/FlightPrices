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

var sln = new Solution();

var chp = sln.FindCheapestPrice(4,flights1, 0,3,1);
var chp1 = sln.FindCheapestPrice(3, flights2, 0, 2, 1);
var chp2 = sln.FindCheapestPrice(3, flights3, 0, 2, 0);
var chp3 = sln.FindCheapestPrice(10, flights4, 6, 0, 7);

var a = chp;

public class Solution
{
    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k)
    {
        var a = GetGraph(flights, src, out Dictionary<int,Node> nodes);
        SetPrices(a, 0, 0, src);

        var ds = nodes[dst];
        var bestPrice = ds.HopsAndPrices.Where(x => x.Key <= k).Select(x => x.Value).OrderBy(x=>x).FirstOrDefault();
        
        return bestPrice == default(int) ? -1 : bestPrice;
    }

    public Node GetGraph(int[][] flights, int src, out Dictionary<int,Node> nodes)
    {
        nodes = new Dictionary<int, Node>();

        foreach (var flight in flights)
        {
            nodes.TryGetValue(flight[0], out var srcNode);

            if (srcNode == null)
            {
                srcNode = new Node() { NodeId = flight[0] };
                nodes.Add(flight[0], srcNode);
            }

            var destination = flight[1];
            var price = flight[2];

            nodes.TryGetValue(destination, out var destNode);

            if (destNode == null)
            {
                destNode = new Node() { NodeId = destination };
                nodes.Add(destination, destNode);

            }

            var newLink = new Link()
            {
                NodeFrom = srcNode,
                NodeTo = destNode,
                Price = price
            };

            srcNode.Children.Add(newLink);
        }

        return nodes[src];

    }

    public void SetPrices(Node node, int stop, int priceToGet, int from)
    {
        foreach (var link in node.Children)
        {
            if (link.NodeTo.CheckedBy.Contains(node.NodeId))
            {
                continue;
            }
            
            if (!link.NodeTo.HopsAndPrices.TryGetValue(stop, out var price))
            {
                link.NodeTo.HopsAndPrices.Add(stop, link.Price + priceToGet);    
            }
            else if(price > priceToGet + link.Price)
            {
                link.NodeTo.HopsAndPrices[stop] = priceToGet + link.Price;
            }
        }

        node.CheckedBy.Add(from); 

        foreach (var link in node.Children.Where(x=>!x.NodeTo.CheckedBy.Contains(node.NodeId)))
        {
            SetPrices(link.NodeTo, stop+1, link.Price, node.NodeId);
        }

    }
    
    
    public class Node
    {
        public Node()
        {
            HopsAndPrices = new Dictionary<int, int>();
            Children = new List<Link>();
            Parent = new List<Link>();
            CheckedBy = new HashSet<int>();
        }

        public HashSet<int> CheckedBy { get; set; }

        public int NodeId { get; set; }
        
        public Dictionary<int, int> HopsAndPrices { get; }
        
        public List<Link> Children { get; set; } 
        
        public List<Link> Parent { get; set; }

        public override string ToString()
        {
            return $"{NodeId}";
        }
    }

    public class Link
    {
        public Link()
        {
        }

        public Node NodeFrom { get; set; }
        
        public Node NodeTo { get; set; }
        
        public int Price { get; set; }

        public override string ToString()
        {
            return $"From: {NodeFrom.NodeId} To: {NodeTo.NodeId} Price: {Price}";
        }
    }
}   
    
