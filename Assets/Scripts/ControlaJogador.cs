﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaJogador : MonoBehaviour, IMatavel, ICuravel
{

	private Vector3 direcao;
	public LayerMask MascaraChao;
	public GameObject TextoGameOver;
	public ControlaInterface scriptControlaInterface;
	public AudioClip SomDeDano;
	private MovementoJogador meuMovimentoJogador;
	private AnimacaoPersonagem animacaoJogador;
	public Status statusJogador;

	void Start () {
		Time.timeScale = 1;
		meuMovimentoJogador = GetComponent<MovementoJogador>();
		animacaoJogador = GetComponent<AnimacaoPersonagem>();
		statusJogador = GetComponent<Status>();
	}


	// Update is called once per frame
	void Update () {
		
		float Xaxis = Input.GetAxis("Horizontal");
		float Zaxis = Input.GetAxis ("Vertical");

		direcao = new Vector3(Xaxis, 0, Zaxis);

		animacaoJogador.Movimentar (direcao.magnitude);
	}

	void FixedUpdate()
	{
		meuMovimentoJogador.Movimentar(direcao, statusJogador.Velocidade);

		meuMovimentoJogador.RotacaoJogador(MascaraChao);
	}

	public void TomarDano (int dano)
	{
		statusJogador.Vida -= dano;
		scriptControlaInterface.AtualizarSliderVidaJogador();
		ControlaAudio.instancia.PlayOneShot(SomDeDano);
		if (statusJogador.Vida <= 0) 
		{
			Morrer();
		}
	}

	public void Morrer()
	{
		scriptControlaInterface.GameOver();
	}

	public void CurarVida (int quantidadeDeCura)
	{
		statusJogador.Vida += quantidadeDeCura;
		if(statusJogador.Vida > statusJogador.VidaInicial)
		{
			statusJogador.Vida = statusJogador.VidaInicial;
		}
		scriptControlaInterface.AtualizarSliderVidaJogador();
	}
}
