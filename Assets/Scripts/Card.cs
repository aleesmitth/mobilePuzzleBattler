public class Card {
    private CardContainer container;
    private string name;

    public Card(string name) {
        this.name = name;
    }

    public void SetContainer (CardContainer container) {
        this.container = container;
    }

    public string GetName() {
        return name;
    }
}