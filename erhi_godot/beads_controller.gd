extends Node3D

@export var Bead_scene: PackedScene

var bead: Node3D
var last_bead: Node3D
var steps_to_forward: int = 0
var vacant_places: Array[float]
var normal_speed: float = 1.0
var speed: float = normal_speed
var high_speed: float = 5.0

func construct(count: int) -> void:
	get_tree().call_group("beads", "queue_free")
	
	var curve = Curve3D.new()
	curve.add_point(Vector3(0.0, 0.0, 0.0), Vector3(5.0, 0.0, 0.0), Vector3(-5.0, 0.0, 0.0))
	curve.add_point(Vector3(0.0, -0.5 * count, 0.0), Vector3(-10.0, 0.0, 0.0), Vector3(10.0, 0.0, 0.0))
	curve.add_point(Vector3(0.0, 0.0, 0.0), Vector3(5.0, 0.0, 0.0), Vector3(-5.0, 0.0, 0.0))
	$Path3D.set_curve(curve)
	
	var bead_set: bool = false
	for i in range(0, count):
		var current = Bead_scene.instantiate()
		$Path3D.add_child(current)
		current.progress_ratio = 1.0 * (count + 5 - i) / (count + 5)
		if bead_set:
			last_bead.next_bead = current
		last_bead = current
		if not bead_set:
			bead = current
			bead_set = true
	
	last_bead.next_bead = self
	vacant_places.clear()
	for i in range(0, 5):
		vacant_places.push_back(1.0 * (i + 1)/(count + 5))
	

func add_vacant_place(place: float) -> void:
	if steps_to_forward > 0 and bead:
		bead.add_vacant_place(place)
		steps_to_forward -= 1
		return
	vacant_places.push_back(place)


func step() -> void:
	if not bead or not last_bead:
		return
	steps_to_forward += 1
	
	if vacant_places.is_empty():
		if speed < high_speed:
			get_tree().call_group("beads", "update_speed", high_speed)
			speed = high_speed
	else:
		if speed > normal_speed:
			get_tree().call_group("beads", "update_speed", normal_speed)
			speed = normal_speed
		
	while not vacant_places.is_empty():
		var n: int = 0
		for place in vacant_places:
			bead.add_vacant_place(place)
			print_debug("step, add_vacant_place:", place)
			n += 1
			
		for i in range(0, n):
			vacant_places.pop_front()
			
	last_bead.next_bead = bead
	last_bead = bead
	bead = bead.next_bead
	last_bead.next_bead = self
