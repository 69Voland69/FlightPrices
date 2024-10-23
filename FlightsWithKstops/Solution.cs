public class Solution
{
    public int FindCheapestPrice(int n, int[][] flights, int src, int dst, int k)
    {
        var a = GetGraph(flights, src, out Dictionary<int,Node> nodes);
        SetPrices(a, 0, 0, src,k, new HashSet<int>());

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
            if (destination == src)
            {
                continue;
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

    

    public void SetPrices(Node node, int stop, int priceToGet, int from, int maxStops, HashSet<int> path)
    {
        if (stop > maxStops || path.Contains(node.NodeId))
        {
            return;
        }

        var linksToExclude = new HashSet<int>();
        foreach (var link in node.Children)
        {
            if (link.NodeTo.HopsAndPrices.Any() && link.NodeTo.HopsAndPrices.All(x => x.Key < stop && x.Value <= link.Price + priceToGet))
            {
                linksToExclude.Add(link.NodeTo.NodeId);
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
        
       path.Add(node.NodeId);
    
        foreach (var link in node.Children)
        {
            if (linksToExclude.Contains(link.NodeTo.NodeId))
            {
                continue;
            }
            SetPrices(link.NodeTo, stop+1, link.Price+priceToGet, node.NodeId,maxStops, path);
        }

        path.Remove(node.NodeId);

    }
    
    
    public class Node
    {
        public Node()
        {
            HopsAndPrices = new Dictionary<int, int>();
            Children = new List<Link>();
            Parent = new List<Link>();
            CheckedBy = new Dictionary<int, int>();
        }

        public Dictionary<int, int> CheckedBy { get; set; }

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