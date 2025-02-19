# Gestion des recettes et des ingrédients

La gestion des recettes et des ingrédients par du principe que tous les éléments sont des ingrédients, et que certains ingrédients peuvent être combinés pour en créer de nouveaux.

Dans cette optique, même le chaudron est un ingrédient. Si aucune recettes ne peut fabriquer un chaudron, il suffit de le mettre dans une recette pour indiquer que cette recette doit être faite dans un chaudron

## Ingrédients

### Créer un nouvel ingrédient :

Chaque ingédient doit :
- être référencé dans l'énumération `IngredientType` du script `IngredientsEnum.cs`
- avoir une préfab portant le même nom exactement que son `IngredientType` et rangée dans le dossier `Prefabs/Ingredients`
- la préfab doit contenir un script qui hérite de `AbstractIngredient.cs`

### Ingrédients spéciaux :

Il est possible de créer des ingrédients qui soient une énum, mais pas une préfab. C'est le cas de l'ingrédient `Caldron`. Cet ingrédient est juste automatiquement ajouté à tous les mélanges qui se font dans le chaudron.

Dans ce cas cet ingrédient ne doit jamais être le résultat d'une recette. Il peut cependant être utilisé dans une recette.

### Détails techniques

Hériter de [AbstractIngredient.cs](../Scripts/Ingredients/AbstractIngredient.cs) permet de bénéficier de la méthode `GetIngredientType()` qui retourne le type de l'ingrédient.
Grâce à ça, quand on est dans un script qui veut merger des ingrédients comme [CaldronMerger.cs](../Scripts/CaldronMerger.cs),
il suffit d'utiliser `GetComponent<AbstractIngredient>()` dans le `OnTriggerEnter` pour obtenir le type de l'ingrédient et s'en servir pour savoir quelles recettes valider ou non.

## Recettes

### Créer une nouvelle recette :

Toutes les recettes sont référencées dans la méthode `GetRecipes()` du script [RecipesManager.cs](../Scripts/RecipesManager.cs).

Pour en ajouter une, il suffit d'ajouter une ligne au dictionary avec en clef le type de l'ingrédient résultat et en valeur la liste des types d'ingrédients nécessaires.

### Détails techniques

La classe [RecipesManager.cs](../Scripts/RecipesManager.cs) est responsable de la gestion des recettes. C'est elle qui recense les recettes et qui les valides.

Elle est également chargé de charger automatiquement les préfabs des ingrédients grâce à leurs noms. Grâce à ce système,
lorsqu'on lui demande de valider une recette, elle renvoie directement la préfab de l'ingrédient résultat et pas seulement son type.
On peut ensuite facilement instancier l'ingrédient résultat.

Il n'y a pas à proprement parlé d'objet 'recette', ce sont en fait des objets de la classe [IngredientList.cs](../Scripts/Ingredients/IngredientList.cs).
Cette classe permet de comparer facilement les listes d'ingrédients entre elles pour savoir si une recette est valide ou non.

Pour ce faire, il a suffi de réimplémenter les méthodes `Equals` (et `GetHashCode`) de la classe [IngredientList.cs](../Scripts/Ingredients/IngredientList.cs)
pour qu'elle retourne `true` si :
- les deux listes ont la même taille
- les deux listes contiennent les mêmes éléments (peu importe l'ordre)

Une `IngredientList` peut contenir plusieurs fois le même ingrédient, dans ce cas, il faut que les deux listes en contiennent autant.