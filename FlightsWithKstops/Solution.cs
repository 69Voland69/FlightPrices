public class Solution
{
    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k)
    {
        var nodes = GetGraph(flights,k);

        if (!nodes.TryGetValue(dst, out _) || !nodes.TryGetValue(src, out _))
        {
            return -1;
        }
        
        Array.Fill(nodes[src].Price.Value,0);
        SetPrices(nodes[src], k);

        var ds = nodes[dst];

        return ds.Price.Value.Min() == Int32.MaxValue ? -1 : ds.Price.Value.Min();
    }

    public Dictionary<int,Node> GetGraph(int[][] flights, int k)
    {
        var nodes = new Dictionary<int, Node>();

        foreach (var flight in flights)
        {
            nodes.TryGetValue(flight[0], out var srcNode);

            if (srcNode == null)
            {
                srcNode = new Node(k) { NodeId = flight[0] };
                nodes.Add(flight[0], srcNode);
            }

            var destination = flight[1];
            var price = flight[2];

            nodes.TryGetValue(destination, out var destNode);

            if (destNode == null)
            {
                destNode = new Node(k) { NodeId = destination };
                nodes.Add(destination, destNode);

            }

            var newLink = new Link()
            {
                NodeTo = destNode,
                Price = price
            };

            srcNode.Children.Add(newLink);
        }

        return nodes;

    }

    public void SetPrices(Node nodeStart, int k)
    {
        var nodes = new Queue<Node>();
        
        nodes.Enqueue(nodeStart);

        for (int i = 0; i <= k; i++)
        {
            
            var newQueue = new Queue<Node>();
            
            while(nodes.TryDequeue(out var node))
            {
                foreach (var link in node.Children)
                {
                    var price = node.Price.Value[(i-1) < 0 ? 0 : i-1] + link.Price;
                    
                    if (link.NodeTo.Price.Value[i] < price)
                    {
                        continue;
                    }

                    link.NodeTo.Price.Value[i] = price;
                    newQueue.Enqueue(link.NodeTo);
                }
            }

            nodes = newQueue;
        }
    }
    
    
    public class Node
    {
        public Node(int k)
        {
            Children = new List<Link>();
            Price = new Lazy<int[]>(() =>
            {
                var a = new int[k+1];
                Array.Fill(a, int.MaxValue);
                return a;
            });
           
        }


        public int NodeId { get; set; }

        public Lazy<int[]> Price { get; set; }
        
        public List<Link> Children { get; set; } 
        
        public override string ToString()
        {
            return $"{NodeId}";
        }
    }

    public class Link
    {
        
        public Node NodeTo { get; set; }
        
        public int Price { get; set; }

        public override string ToString()
        {
            return $"To: {NodeTo.NodeId} Price: {Price}";
        }
    }
}