using GeneticCars.Cars;
using GeneticCars.Evolution;

namespace GeneticCars.Genealogy;

public partial class FamilyTree
{
    private (int g, int i, bool sticky)? _selected;

    public List<NodesGeneration> Generations { get; } = new(100);

    public void UpdateScoredGeneration(Generation<Car> carGeneration)
    {
        Node[] updatedPopulation;
        if (Generations.Count > 1) { // Unscored generation is already there
            Node[] ancestors = Generations[^2].Population;
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            updatedPopulation = carGeneration.Population
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i =>
                    new Node(i, GetIndex(i.Ancestor1, indices), GetIndex(i.Ancestor2, indices), FitnessHandling.Known))
                .ToArray();
        } else {
            updatedPopulation = carGeneration.Population
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i => new Node(i, null, null, FitnessHandling.Known))
                .ToArray();
        }
        Generations[^1].Population = updatedPopulation;
    }

    public void AddUnscoredGeneration(Generation<Car> carGeneration)
    {
        Node[] newPopulation;
        if (Generations.Count > 0) {
            Node[] ancestors = Generations[^1].Population;
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            newPopulation = carGeneration.Population
                .Select(ind => new Node(ind, GetIndex(ind.Ancestor1, indices), GetIndex(ind.Ancestor2, indices),
                    ind.Class == Class.Elite && !carGeneration.NewFloorGenerated
                        ? FitnessHandling.Inherit
                        : FitnessHandling.Unknown))
                .ToArray();
        } else {
            newPopulation = carGeneration.Population
                .Select(ind => new Node(ind, null, null, FitnessHandling.Unknown))
                .ToArray();
        }
        Generations.Add(new NodesGeneration(newPopulation, carGeneration));
    }

    public bool SelectNode(int x, int y, bool sticky)
    {
        if (sticky || _selected is not { sticky: true }) {
            float x1 = (x / _zoom - HorizontalBorder) / (TextColumnWidth + ConnectionsColumnWidth);
            int ix = (int)Single.Floor(x1);
            bool inTextPart = (x1 - ix) * (TextColumnWidth + ConnectionsColumnWidth) <= TextColumnWidth;

            int iy = (int)Single.Floor((y / _zoom - TopBorder + LineHeight - 4) / LineHeight);

            (int g, int i, bool sticky)? newNode =
               inTextPart && ix >= 0 && ix < Generations.Count && iy >= 0 && iy < Generations[ix].Population.Length
                ? (ix, iy, sticky && _selected != (ix, iy, true))
                : null;
            if (newNode != _selected) {
                _selected = newNode;
                return true;
            }
        }
        return false;
    }

    public bool DeselectNode()
    {
        if (_selected is { sticky: false }) {
            _selected = null;
            return true;
        }
        return false;
    }

    private bool IsRelatedToSelection(int g, int i, int? ancestorNum)
    {
        if (_selected == null) {
            return false;
        }

        if (g == _selected.Value.g) {
            return i == _selected.Value.i;
        }
        if (g < _selected.Value.g) {
            return IsRelatedAncestor(g + 1, i);
        }

        Node node = Generations[g].Population[i];
        return
            ancestorNum is null or 1 && node.Ancestor1Index is int i1 && IsRelatedDescendant(g - 1, i1) ||
            ancestorNum is null or 2 && node.Ancestor2Index is int i2 && IsRelatedDescendant(g - 1, i2);
    }

    private bool IsRelatedDescendant(int ofG, int i)
    {
        EnsureCashIsActual(ofG);
        return Generations[ofG]._relatedNodesCash.Contains(i);
    }

    private void EnsureCashIsActual(int g)
    {
        NodesGeneration generation = Generations[g];
        (int g, int i) selected = (_selected!.Value.g, _selected.Value.i);
        if (generation._cashedSelection != selected) {
            generation._cashedSelection = selected;

            generation._relatedAncestorsCash.Clear();
            generation._relatedNodesCash.Clear();
            if (g == selected.g) {
                Node node = generation.Population[selected.i];
                if (node.Ancestor1Index is int i1) {
                    generation._relatedAncestorsCash.Add(i1);
                    if (node.Ancestor2Index is int i2) {
                        generation._relatedAncestorsCash.Add(i2);
                    }
                }
                generation._relatedNodesCash.Add(selected.i);
            } else if (g < selected.g) {
                for (int i = 0; i < generation.Population.Length; i++) {
                    if (IsRelatedAncestor(g + 1, i)) {
                        Node node = generation.Population[i];
                        if (node.Ancestor1Index is int i1) {
                            generation._relatedAncestorsCash.Add(i1);
                            if (node.Ancestor2Index is int i2) {
                                generation._relatedAncestorsCash.Add(i2);
                            }
                        }
                    }
                }
            } else { // g > selectedG
                for (int i = 0; i < generation.Population.Length; i++) {
                    Node node = generation.Population[i];
                    if (node.Ancestor1Index is int i1 && IsRelatedDescendant(g - 1, i1) ||
                        node.Ancestor2Index is int i2 && IsRelatedDescendant(g - 1, i2)) {

                        generation._relatedNodesCash.Add(i);
                    }
                }
            }
        }
    }

    private bool IsRelatedAncestor(int ofG, int i)
    {
        EnsureCashIsActual(ofG);
        return Generations[ofG]._relatedAncestorsCash.Contains(i);
    }

    private static int? GetIndex(Individual? ancestor, Dictionary<string, int> indices) =>
        ancestor is null ? null : indices.TryGetValue(ancestor.Identity.InfoText, out int index) ? index : null;
}
