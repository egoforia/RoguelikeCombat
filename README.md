# Roguelike Combat System Prototype
*A tactical roguelike with Vagrant Story-inspired combat mechanics*

## Project Overview
A roguelike game featuring precise combat mechanics, chain abilities, and tactical body-part targeting. Built in Unity3D with a focus on tight, responsive controls and clear player feedback.

## Core Game Design

### Session Structure
- Run duration: 15-30 minutes
- Permadeath system
- Persistent progression with decay mechanics
  - Skills and stats persist between runs
  - 10-15% decay on death

### Combat System

#### Timing Mechanics
- **Base Timing Window**: 500ms (configurable)
- **Hit Categories**:
  - Perfect: Center of window
  - Good: Outer edges
  - Miss: Outside window

#### Chain System
- **Damage Multipliers**
  - Base hit: 1.0x
  - Good timing: 1.2x
  - Perfect timing: 1.5x
  - Perfect chain bonus: +0.2x per hit (caps at 2.5x)

#### Critical Hit System
- Base chance: 5%
- Good timing: +5% (10% total)
- Perfect timing: +10% (15% total)
- Chain bonus: +3% per chain hit

#### Body-Part Targeting
| Body Part | Effect                    | Trade-off                    |
|-----------|---------------------------|------------------------------|
| Head      | High damage, critical hits| Smaller target              |
| Arms      | Disable enemy attacks     | Medium damage               |
| Legs      | Reduce movement/evasion   | Lower damage                |
| Torso     | Balanced damage          | Larger target               |

### Visual Feedback

#### UI Elements
1. **Chain Meter**
   - Circular indicator around character
   - Color-coded segments (Green/Yellow/Red)
   - Central chain counter

2. **Timing Bar**
   - Horizontal bar above character
   - Moving marker for optimal timing
   - Visual zone indicators
   - Hit feedback effects

3. **Combat Feedback**
   - Color-coded damage numbers
   - Chain-based visual effects
   - Perfect chain screen effects
   - Critical hit stop-time effect

### Progression System

#### Timing Window Skills
**Quick Reflexes Tree:**
- Level 1: +50ms window
- Level 2: +100ms window
- Level 3: +150ms window
- Master: Perfect window increased by 25%

#### Chain Enhancement
**Chain Mastery:**
- Increased chain potential
- Reduced timing degradation
- Enhanced damage scaling
- Chain recovery mechanics

## Technical Implementation

### Required Unity Packages
- Input System
- TextMeshPro
- Universal Render Pipeline

### Core Scripts Structure

Assets/
├── _Project/
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Models/
│   │   └── Textures/
│   ├── Prefabs/
│   │   ├── Characters/
│   │   ├── UI/
│   │   └── VFX/
│   ├── Scenes/
│   │   ├── TestCombat.unity
│   │   └── MainGame.unity
│   └── Scripts/
│       ├── Combat/
│       ├── UI/
│       └── Progression/
├── Settings/
└── ThirdParty/

## Development Roadmap

### Phase 1: Core Combat
- [ ] Basic player controller
- [ ] Timing system implementation
- [ ] Chain system core mechanics
- [ ] Basic hit detection

### Phase 2: Visual Feedback
- [ ] Chain meter UI
- [ ] Timing bar implementation
- [ ] Hit effects and particles
- [ ] Damage number system

### Phase 3: Progression
- [ ] Skill system framework
- [ ] Persistence between runs
- [ ] Skill tree UI
- [ ] Death penalty system

### Phase 4: Polish
- [ ] Combat feel and juice
- [ ] UI animations
- [ ] Sound effects
- [ ] Performance optimization

## Getting Started
1. Clone this repository
2. Open in Unity 2022.3 LTS or newer
3. Install required packages
4. Open the test scene in `Assets/Scenes/TestCombat`
