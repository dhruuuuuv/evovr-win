﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using System.Linq;

using LibPDBinding;

using Combinatorial;

//class to manage instruments and objects
public class GameControl : MonoBehaviour {

//	Vector3 base_position = new Vector3(1.82f, 8.29f, -13.95f);
	public GameObject inst_prefab; 
	public GameObject inst;
	public Rigidbody instrument;

	public ShowText gui_text;

	public ShowText genome_text;

	public GameObject libpd;

	private int count_before_evolution = 4;
//	private int instrument_number;

	public float save_x_min = -3.5f;
	public float save_x_max = 12.5f;

	public float save_z_min = 1.5f;
	public float save_z_max = 17.5f;

	private bool first_generation;

	public List<List<List<int>>> saved_genomes;
	public List<List<List<int>>> genomes_to_load;


	private LibControl lib_control;
	private Genome current_genome;

	int generation;

	private int child_index = 0;


	void Awake () {
		libpd = GameObject.Find ("LibPD");
		GameObject canvas = GameObject.Find ("Canvas");
		gui_text = canvas.GetComponent<ShowText> ();

		GameObject genomecanvas = GameObject.Find ("GenomeCanvas");
		genome_text = genomecanvas.GetComponent<ShowText> ();

//		lib_control = libpd.GetComponent<Control>();
	}

	// Use this for initialization
	void Start () {
		//		make a new instrument
//		Instantiate(inst_prefab, base_position, Quaternion.identity, true);

		saved_genomes = new List<List<List<int>>> ();
		genomes_to_load = new List<List<List<int>>> ();

		gui_text.enable_text ("Generation: 1, first instrument");

		generation = 1;

		inst = Instantiate (inst_prefab);

		inst.GetComponent <MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

		instrument = inst.GetComponent<Rigidbody> ();

//		Genome gen = new Genome (instrument);
//		lib_control = new LibControl (instrument, gen);
	
		lib_control = inst.AddComponent<LibControl>();

		current_genome = lib_control.get_genome ();

		Debug.Log (genome_string(genome_to_list()));

		genome_text.enable_text (genome_string(genome_to_list()));

		Debug.Log ("maybe it's a lib control error");
		Debug.Log (current_genome);

		first_generation = true;

//		instrument_number = 1;
	}
	
	// Update is called once per frame
	void Update () {
		genome_text.enable_text (genome_string(genome_to_list()));	
	}

//	public void Save () {
//		BinaryFormatter bf = new BinaryFormatter ();
//		FileStream file = File.Create (Application.persistentDataPath + "/genome.dat");
//
////		Genome g = new Genome (instrument);
////		g.blah = ...
//
//		bf.Serialize (file, current_genome);
//
//		file.Close();
//
//	}
//
//	public void Load() {
//		if (File.Exists (Application.persistentDataPath + "/genome.dat")) {
//			BinaryFormatter bf = new BinaryFormatter ();
//			FileStream file = File.Open (Application.persistentDataPath + "genome.dat", FileMode.Open);
//		
//			Genome g = (Genome)bf.Deserialize(file);
////			health = g.health ...
//
//		}
//
//	}

	public string genome_string(List<List<int>> genome) {
		string gen = string.Empty;

		foreach (List<int> intlist in genome) {
			foreach (int param in intlist) {
				gen = gen + param.ToString();
				gen = gen + " ";
			}
		}

		return gen;
	}
		

	public List<List<int>> genome_to_list() {
		List<List<int>> dna = new List<List<int>> ();

		List<int> subdna = new List<int> ();

		Debug.Log (subdna);
		Debug.Log(current_genome);
		subdna.Add (current_genome.metro_env_filter);
		subdna.Add (current_genome.receiver_index);
		subdna.Add (current_genome.rb_property_index);

		dna.Add (subdna);

		List<int> subdna2 = new List<int> ();
		subdna2.AddRange (current_genome.env_gen);
		dna.Add (subdna2);

		List<int> subdna3 = new List<int> ();
		subdna3.AddRange (current_genome.filter_gen);
		dna.Add (subdna3);

		List<int> subdna4 = new List<int> ();
		subdna4.AddRange (current_genome.metro_gen);
		dna.Add (subdna4);

		return dna;
	}

	public List<List<int>> random_genome() {
		List<List<int>> dna = new List<List<int>> ();

		List<int> subdna = new List<int> ();

		Genome random_genome = new Genome ();

		subdna.Add (random_genome.metro_env_filter);
		subdna.Add (random_genome.receiver_index);
		subdna.Add (random_genome.rb_property_index);

		dna.Add (subdna);

		List<int> subdna2 = new List<int> ();
		subdna2.AddRange (random_genome.env_gen);
		dna.Add (subdna2);

		List<int> subdna3 = new List<int> ();
		subdna3.AddRange (random_genome.filter_gen);
		dna.Add (subdna3);

		List<int> subdna4 = new List<int> ();
		subdna4.AddRange (random_genome.metro_gen);
		dna.Add (subdna4);

		return dna;
	}
		

//	check if the instrument object is in the green area, and if so, save
	public void save_instrument() {


//		float x_pos = instrument.position.x;
//		float z_pos = instrument.position.z;

//		if (x_pos > save_x_max && x_pos < save_x_min && z_pos > save_z_min && z_pos < save_z_max) {
			saved_genomes.Add(genome_to_list ());

//		} else {
//			DestroyObject (instrument);
//		}

	}

	public void destroy_instrument() {
		DestroyObject (inst);

//		does destroying the libpd object work mid program?
//		may have to just reassign the object 'tracker'

//		IGNORE ABOVE FOR NOW, JUST REASSIGNING LIB CONTROL
//		DestroyObject(lib_control);

	}

//	create a new instrument with a new genome if the number of instruments is below a threshold
	public void new_instrument() {

		Debug.Log("Making a new instrument!");

//		for now, always destroy the last instrument
		destroy_instrument();
		
//		check if it's the first generation, and then if so, don't load any genomes
		if (generation == 1) {
			
//			check the number of saved is <= to the threshold for next generation
//			if so make new instrument
		

			if (saved_genomes.Count <= count_before_evolution) {

				gui_text.enable_text ("New instrument made!");

				inst = Instantiate (inst_prefab);

				inst.GetComponent <MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

				instrument = inst.GetComponent<Rigidbody> ();

//				Genome gen = new Genome (instrument);
//				lib_control = new LibControl (instrument, gen);

				lib_control = inst.AddComponent<LibControl> ();

				current_genome = lib_control.get_genome ();
//				instrument_number += 1;

//			otherwise if the number of instruments has surpassed threshold
//			need to assign saved genomes to other List, and load first genome
			} else {
				

				//			mutate_genome ();
				generation = generation + 1;
				child_index = 0;

				gui_text.enable_text ("Generation: " + generation + ". Genome mutated.");

				Debug.Log ("Generation: " + generation);

				genomes_to_load = mutate_genome ();

//				blank out saved genomes, so that can assign new genomes to it
				saved_genomes = new List<List<List<int>>> ();

				load_genome (genomes_to_load, child_index);
				child_index++;

			}


//		otherwise the genome needs to be loaded from the mutated list
		} else {
			
			//			check if limit has been received, and if not, we're just loading the next instrument
			if (child_index < genomes_to_load.Count) {

				gui_text.enable_text ("instrument number " + child_index + "loaded.");

				
				load_genome (genomes_to_load, child_index);
				child_index++;
			}

//			otherwise we need to move on to another generation
//			for now this just happens when all the mutated instruments have been tested
//			but could potentially add some random instruments in there, just test and see perhaps.
			else {

				generation++;

				Debug.Log ("Generation: " + generation);

				child_index = 0;

				genomes_to_load = mutate_genome ();

//				blank out saved genomes, so that can assign new genomes to it
				saved_genomes = new List<List<List<int>>> ();

				load_genome (genomes_to_load, child_index);
				child_index++;
			}
		}

	}

//	loads genome from list of saved children, and the index taken from the 'child index' parameter
	public void load_genome(List<List<List<int>>> children, int index) {

		Debug.Log ("Genome is being loaded from saved genomes");

		Debug.Log ("child index");
		Debug.Log (child_index);

		Debug.Log ("saved_genomes count");
		Debug.Log (children.Count);

		List<List<int>> child = children [index];

		inst = Instantiate (inst_prefab);

		inst.GetComponent <MeshRenderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));

		instrument = inst.GetComponent<Rigidbody> ();

		Genome gen = new Genome (instrument, child [0], child [1], child [2], child [3]);
//		lib_control = new LibControl (instrument, gen);

		lib_control = inst.AddComponent<LibControl> ();

		Debug.Log ("Assigning genome to lib_control");
		lib_control.instrument_genome = gen;

		current_genome = lib_control.get_genome ();
	
		
	}

	public List<List<List<int>>> mutate_genome() {

		Debug.Log ("Genomes are being mutated");

		var children = new List<List<List<int>>> ();

//		foreach (List<int> genome in saved_genomes) {
//			condensed_list.AddRange (genome);
//		}

		int genome_length = saved_genomes.Count;

//		Combinations genome_combinations = new Combinations(saved_genomes, 2);

		if (genome_length % 2 == 0) {
		
			for (int i = 0; i <= ((genome_length - 1) / 2); i += 2) {
				crossover (saved_genomes [i], saved_genomes [i+1], children);

			}
		
		}

		else {

//			wrap around so first is paired with last initially

			crossover(saved_genomes[0], saved_genomes[genome_length - 1], children);

			for (int i = 0; i <= ((genome_length - 1) / 2); i += 2) {
				crossover (saved_genomes [i], saved_genomes [i+1], children);

			}
		}
	
		return children;
			
	
	}

//	implementation of the crossover algorithm
	public void crossover(List<List<int>> mother, List<List<int>> father, List<List<List<int>>> children) {
		
		var child1 = new List<List<int>> ();
		var child2 = new List<List<int>> ();

		child1.Add (mother [0]);
		child1.Add (mother [1]);
		child1.Add (father [2]);
		child1.Add (father [3]);

		child2.Add (father [0]);
		child2.Add (father [1]);
		child2.Add (mother [2]);
		child2.Add (mother [3]);


		children.Add (child1);
		children.Add (child2);

	}


}
