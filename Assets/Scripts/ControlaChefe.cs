﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ControlaChefe : MonoBehaviour, IMatavel
{

	private Transform jogador;
	private NavMeshAgent agente;
	private Status statusChefe;
	private AnimacaoPersonagem animacaoChefe;
	private MovementoPersonagen movimentoChefe;
	public GameObject KitMedicoPrefab;
	public Slider sliderVidaChefe;
	public Image ImagelSlider;
	public Color CorDaVidaMaxima, CorDaVidaMinima;
	public GameObject ParticulaSangueZumbi; 

	private void Start()
	{
		jogador = GameObject.FindWithTag("Jogador").transform;
		agente = GetComponent<NavMeshAgent>();
		statusChefe = GetComponent<Status>();
		agente.speed = statusChefe.Velocidade;
		animacaoChefe = GetComponent<AnimacaoPersonagem>();
		movimentoChefe = GetComponent<MovementoPersonagen>();
		sliderVidaChefe.maxValue = statusChefe.VidaInicial;
		AtualizarInterface(); 

	}

	private void Update()
	{
		agente.SetDestination(jogador.position);
		animacaoChefe.Movimentar(agente.velocity.magnitude);

		if (agente.hasPath == true) 
		{
			bool estouPertoDoJogador = agente.remainingDistance <= agente.stoppingDistance;

			if (estouPertoDoJogador) 
			{
				animacaoChefe.Atacar (true);
				Vector3 direcao = jogador.position - transform.position;
				movimentoChefe.Rotacionar(direcao);
			} 
			else 
			{
				animacaoChefe.Atacar (false);
			}
		}
	}

	void AtacaJogador ()
	{
		int dano = Random.Range(30, 40);
		jogador.GetComponent<ControlaJogador>().TomarDano(dano);
	}

	public void TomarDano(int dano)
	{
		statusChefe.Vida -= dano;
		AtualizarInterface();
		if (statusChefe.Vida <= 0) 
		{
			Morrer();
		}
	}

	public void Morrer()
	{
		animacaoChefe.Morrer();
		movimentoChefe.Morrer();
		this.enabled = false;
		agente.enabled = false;
		Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
		Destroy(gameObject, 2);
	}

	public void ParticulaSangue (Vector3 posicao, Quaternion rotacao)
	{
		Instantiate(ParticulaSangueZumbi, posicao, rotacao);
	}


	void AtualizarInterface ()
	{
		sliderVidaChefe.value = statusChefe.Vida;
		float porcentagemDaVida = (float)statusChefe.Vida / statusChefe.VidaInicial;
		Color corDaVida = Color.Lerp(CorDaVidaMinima, CorDaVidaMaxima, porcentagemDaVida);
		ImagelSlider.color = corDaVida;
	}
}
