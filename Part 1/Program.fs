open System
open System.IO
open System.Windows.Forms
open System.Drawing


/// The type that represents each row of the training or test dataset
type Entry = {
    Label : string
    Values: int list
}

/// Calculates the squared euclidean distance between pixel values of 2 images
let distance (values1 : int list, values2 : int list) =
    values1
    |> List.zip values2
    |> List.map (fun it -> Math.Pow(float (fst it) - float (snd it), 2.0))
    |> List.sum

/// Loading values from the training or test data.
let loadValues (fileName : string) =
    File.ReadAllLines(fileName)
        |> Seq.ofArray
        |> Seq.skip(1)
        |> Seq.map (fun line ->
            {
                Label = line.Substring(0,line.IndexOf(','));
                Values = line.Split(',')
                    |> Seq.ofArray
                    |> Seq.skip(1)
                    |> Seq.map(fun n -> Convert.ToInt32(n))
                    |> Seq.toList
            }
        ) |> Seq.toList

/// A generic k-nearest neighbour algorithm
let kNN (entries: Entry list, newEntry : string * int[], k : int) =
    entries
        |> List.map (fun x -> (x.Label, distance (x.Values, snd (newEntry) |> Array.toList)))
        |> List.sortBy (fun x -> snd x)
        |> Seq.ofList
        |> Seq.take k
        |> Seq.countBy (fun x -> fst x)
        |> Seq.toList

/// Draws the digit
let drawDigit (pixels : float[], label : string) =
    let tile = 20
    let form = new Form(TopMost = true,Visible = true,Width = 29 * tile,Height = 29 * tile)

    let panel = new Panel(Dock = DockStyle.Fill)
    panel.BackColor <- Color.Black
    form.Controls.Add(panel)
    let graphics = panel.CreateGraphics()
    pixels
    |> Array.iteri ( fun i p ->
        let col = i % 28
        let row = i /28
        let color = Color.FromArgb(int p, int p, int p)
        let brush = new SolidBrush(color)
        graphics.FillRectangle(brush, col * tile, row * tile, tile, tile))
    let point = new PointF((float32)5, (float32)5)
    let font = new Font(family = FontFamily.GenericSansSerif, emSize = (float32)30)
    graphics.DrawString(label, font, new SolidBrush(Color.YellowGreen), point)
    form.Show()

[<EntryPointAttribute>]
let main argv =
    let loaded = loadValues @"D:\Documents\Education\Programming\Visual Studio\Visual F#\Machine Learning\train.csv"
    let newEntry = ("X", [|0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;25;123;245;243;211;45;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;71;227;252;166;47;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;20;197;249;223;47;2;68;232;98;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;48;202;252;155;35;0;15;225;252;80;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;190;252;155;7;0;0;110;252;208;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;123;253;109;0;0;0;68;245;216;18;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;253;224;14;0;0;15;211;252;153;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;54;253;71;0;0;9;237;252;224;7;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;106;253;111;37;91;204;253;252;126;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;18;253;252;235;252;208;253;252;82;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;62;106;106;35;0;255;204;9;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;80;253;89;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;158;253;63;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;43;239;225;21;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;64;252;167;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;169;253;45;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;8;197;252;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;22;252;244;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;22;252;173;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;13;217;121;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0|])
    let pixels = snd(newEntry) |> Array.map(fun t -> float t)
    let k = 5
    
    let labels = kNN(loaded, newEntry, k)
    let guess = fst(labels |> List.item 0)
    drawDigit (pixels, "I think it is: " + guess)
    Console.ReadLine() |> ignore
    0