﻿using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

using LibPDBinding;

public class LibControl : MonoBehaviour {

	public Rigidbody rb;
//	public float freq_factor = 100;
//
//	public float min_x_lim = -30;
//	public float max_x_lim = 30;

	public Genome instrument_genome;

//	void Start()
//	{
//		instrument_genome = new Genome (rb);
//		send_init_parameters ();
//	
//	}

//	public LibControl(Rigidbody rigidb, Genome gen) {
//
//		Debug.Log ("Libcontrol been made");
//
//		rb = rigidb;
//
//		instrument_genome = gen;
//		send_init_parameters ();
//
//	}

	public LibControl() {

//		Debug.Log ("Libcontrol been made");

	}

	void Awake () {
		rb = gameObject.GetComponent<Rigidbody> ();
		Genome gen = new Genome (rb);

		instrument_genome = gen;

//		Debug.Log (instrument_genome);

	}

	void Start () {
		Debug.Log ("about to send init parameters, does this need to come later, in awake?");

		send_init_parameters ();
	}

	// Update is called once per frame
	void Update () {

//		Debug.Log ("LibControl Update");


		float prop_float = instrument_genome.get_property_float ();

		string pd_receive = instrument_genome.get_pd_string ();

//		Debug.Log ("pd_receive");
		Debug.Log (pd_receive);
//		Debug.Log ("instrument_genome.rb_property_index");
		Debug.Log (instrument_genome.rb_property_index);
//		Debug.Log ("prop_float");
//		Debug.Log (prop_float);

		LibPD.SendFloat (pd_receive, prop_float);

	}

	public Genome get_genome() {
		return instrument_genome;
	}


//	sends all the non controlled randomised parameters to the various receivers in pd
	void send_init_parameters() {

//		LibPD.SendFloat ("volume", 0.7f);

		for (int i = 0; i < instrument_genome.filter_gen.Length; i++) {
			LibPD.SendFloat (instrument_genome.filter [i], instrument_genome.filter_gen [i]);
		}

		for (int i = 0; i < instrument_genome.env_gen.Length; i++) {
			LibPD.SendFloat (instrument_genome.env [i], instrument_genome.env_gen [i]);
		}

		for (int i = 0; i < instrument_genome.metro_gen.Length; i++) {
			LibPD.SendFloat (instrument_genome.metro [i], instrument_genome.metro_gen [i]);
		}

//		check if the user is controlling the frequency of the instrument_genome
//		if (instrument_genome.metro_on == 1) {
//			LibPD.SendFloat ("metro-on", 1);
//
//		} else {
//			LibPD.SendFloat ("metro-on", 0);
//		}

	}
		





}
	