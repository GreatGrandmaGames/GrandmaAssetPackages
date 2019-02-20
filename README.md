# Grandma Code Base Docs

## Introduction
This README gives an overview of the GrandmaAssetPackages/Core, referred to as the Core. 

The Core is the basis for any object which stores some game state. It provides a system to serialise provided data, meaning game state can be loaded / saved, sent over a network etc. Laying out every object that stores game state like this allows greater control over system design, the leveraging of inheritance and easier debugging. Any class that does not conform to this pattern should have a well documented reason as to why it does not inherit from a provided class. 

## Core Overview

### Upstream
Upstream classes are the top of the inheritance structure.

#### `GrandmaObject : MonoBehaviour`

- A GrandmaObject is an object that stores some state. It has a data class to store a name and a unique ID. 
- A GrandmaObject will register itself with the GrandmaObjectManager (GOM) on Awake. It can Read (deserialise, load) and Write (serialise, save) itself and attached GrandmaComponents. Writing is the transfer of game state to JSON / byte stream; reading is the transfer of JSON / byte stream to game state.

#### `GrandmaObjectManager: MonoBehaviour`
The GrandmaObjectManager is a Singleton. It can be used to:
1. Register / Unregister GrandmaObjects
  a. It stored every GrandmaObject currently in the scene
  b. It assigns the GrandmaObject a unique ID generator using the GrandmaIDGenerator. (TODO: use built-in GUID and doc)
2. Get a GrandmaObject by ID
3. Get a GrandmaComponent by ID

#### `GrandmaComponent: MonoBehaviour < abstract`
The GrandmaComponent is the base class for all game state components. It has an associated data class called GrandmaComponentData < serialisable. All data classes used for inheriting components should inherit from this data class. On Awake, it will:
1. Link itself to its GrandmaObject (it will attach the component if necessary). 
2. Register its linked GrandmaObject if necessary.
3. Read any provided data. If no data is provided, it will throw an error.

The GrandmaComponent can:
1. Read provided GrandmaComponentData. Inheriting classes should implement OnRead(GrandmaComponentData) to gain control at this time. Data classes should be downcast appropriately by their inheriting classes. External classes should listen for the OnUpdated<GrandmaComponent> event.
2. Write to JSON. (TODO: write to Byte stream). All serialisable fields in the data class will be written. Inheriting classes can override OnWrite() to adjust any data class fields based on Unity game object state if needed here. 
3. Validate their state. This checks that their data exists and is valid. Any additional validity checks can be implemented in subclasses.
  
#### `GrandmaComponentData : ScriptableObject`

GrandmaComponentData is the base class for every data class that a GrandmaComponent uses. They are serialised at Write time and deserialised at Read time. (TODO: GrandmaComponentData defines an overridable way to add themselves together without destroying either object - for the purpose of buffs / debuffs).

## Core Functionality Classes
The Core also provides some common functionality for games. These classes also serve as a useful references for using the Grandma system layout. 

By convention, data classes should be named “[FunctionalityClassName]Data”, and should share the same namespace. Functionality classes should not end with the word “Data”. If you are using the CreateAssetMenu attribute, the menuName should be in the form: "[PackageName]/[FunctionalityClassName or clear abbreviation][whitespace for clarity]Data".
Any class with an associated data class (ADC), which inherits from GrandmaComponentData, will be marked with < ADC. Any class using an ADC that is not exclusive to them (e.g. a class inheriting from a class using an ADC that adds no game state data), is tagged with ADC([DataClasseName]).

OnRead and OnWrite overrides are mentioned only where something more interesting than the basic copying of values takes place.

### Common GrandmaComponents

#### `Damageable: GrandmaComponent < ADC`
A Damageable object is anything that can be wounded or destroyed. Examples include: player, enemy, explosive barrel. They can be Damaged or Healed by some amount - optionally by a GrandmaAgent and optionally by some source (e.g. a weapon).

(TODO) When a Damageable object dies, it is Destroyed - optionally by a GrandmaAgent and optionally by some source (e.g. a weapon).

#### `Positionable : GrandmaComponent < ADC`
A Positionable object can be placed somewhere in a scene. It will Write / Read its position, rotation and local scale.

#### `Moveable: Positionable < ADC`
PUT IMAGE

### Agents

#### `Agent: GrandmaComponent < ADC`
(DOC TODO:) Belongs to faction

#### `Faction: GrandmaComponent < ADC`
(DOC TODO:) Has many agents, name, etc.

#### `AgentItem: GrandmaComponent < ADC`
(DOC TODO:) Belongs to an agent

## TODO

