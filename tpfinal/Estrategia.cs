
using System;
using System.Collections.Generic;
using tp1;

namespace tpfinal
{

	public class Estrategia
	{
		private int CalcularDistancia(string str1, string str2)
		{
			// using the method
			String[] strlist1 = str1.ToLower().Split(' ');
			String[] strlist2 = str2.ToLower().Split(' ');
			int distance = 1000;
			foreach (String s1 in strlist1)
			{
				foreach (String s2 in strlist2)
				{
					distance = Math.Min(distance, Utils.calculateLevenshteinDistance(s1, s2));
				}
			}

			return distance;
		}

		public String Consulta1(ArbolGeneral<DatoDistancia> arbol)
		{
			List<string> hojasEncontradas = new List<string>();

			busquedaHojas(arbol, hojasEncontradas);

			return String.Join("\n", hojasEncontradas);
		}


		public String Consulta2(ArbolGeneral<DatoDistancia> arbol)
		{
			List <string> caminosEncontrados= new List<string>();

			recoleccionCaminos(arbol,arbol.getDatoRaiz().texto,caminosEncontrados);

            return String.Join("\n", caminosEncontrados);
		}
        public string Consulta3(ArbolGeneral<DatoDistancia> arbol)
        {
            System.Text.StringBuilder stringContenedor = new System.Text.StringBuilder();

            Cola<ArbolGeneral<DatoDistancia>> cola = new Cola<ArbolGeneral<DatoDistancia>>();

            cola.encolar(arbol);
            int nivelActual = 0;

            while (cola.cantidadElementos() != 0)
            {
                int nodosNivel = cola.cantidadElementos();

                stringContenedor.AppendLine($"Nivel : {nivelActual}");

                for (int i = 0; i < nodosNivel; i++)
                {
                    ArbolGeneral<DatoDistancia> nodoActual = cola.desencolar();

                    stringContenedor.Append(nodoActual.getDatoRaiz().texto + " ||| ");

                    foreach (var hijo in nodoActual.getHijos())
                    {
                        cola.encolar(hijo);
                    }
                }
                stringContenedor.AppendLine();
                nivelActual++;
            }
            return stringContenedor.ToString();
        }

        public void AgregarDato(ArbolGeneral<DatoDistancia> arbol, DatoDistancia dato)
		{
			//Implementar o TO DO ----> Realizado.
			//Se chequea que el arbol no este vacio, caso contrario se agrega el dato siendo este la raiz del arbol.

			if (arbol.getDatoRaiz() == null)
			{
				arbol.DATO = dato;
			}

			// Si el arbol no esta vacio se pasa a la siguiente etapa
			else
			{
				// Se procede a calcular la distancia entre el dato a agregar y el dato que se encuentra en la raiz
				int distancia = CalcularDistancia(arbol.getDatoRaiz().texto, dato.texto);

				// Se iniciliaza una variable de tipo bool en false, considerada como flag para determinar si es que se encontro un hijo con la misma distancia que el dato a agregar
				bool estadoBusqueda = false;

				foreach (ArbolGeneral<DatoDistancia> hijoPuntero in arbol.getHijos())
				{
					if (hijoPuntero.getDatoRaiz().distancia == distancia)
					{
						// Si entra al if significa que se encontro un hijo con la misma distancia que el dato a agregar por ende la bandera pasa a ser true
						estadoBusqueda = true;

						//Se procede a llamar recursivamente al metodo AgregarDato para agregar el dato en el hijo que tiene la misma distancia
						this.AgregarDato(hijoPuntero, dato);
						break;
					}
				}

				if (estadoBusqueda == false)
				{
					arbol.agregarHijo(new ArbolGeneral<DatoDistancia>(new DatoDistancia(distancia, dato.texto, dato.descripcion)));
				}
			}
		}

		public void Buscar(ArbolGeneral<DatoDistancia> arbol, string elementoABuscar, int umbral, List<DatoDistancia> collected)
		{
			//Implementar TO DO

			//Se verifica que el arbol no este vacio,caso afirmativo se retorna de inmediato.
			if (arbol.getDatoRaiz() == null)
			{
				return;
			}
			else //Si el arbol no esta vacio se pasa a realizar la logica de busqueda
			{
				//Se calcula la distancia entre el Elemento a buscar y el dato que se encuentra en la raiz
				int distancia = this.CalcularDistancia(elementoABuscar, arbol.getDatoRaiz().texto);

				// Se consigue un rango minimo y maximo entre los cuales se acepta la distancia, de esta manera se aprovecha la estructura de datos para NO BUSCAR ENTRE TODOS LOS HIJOS.
				int distanciaMinima = distancia - umbral;
				int distanciaMaxima = distancia + umbral;

				if (distancia <= umbral)
				{
					// Obtenemos el dato del nodo actual
					DatoDistancia datoActual = arbol.getDatoRaiz();

					// Creamos un NUEVO objeto para la lista de resultados
					// Le pasamos la 'distancia' que acabamos de calcular (la distancia al término de búsqueda)
					// junto con el texto y la descripción originales.
					collected.Add(new DatoDistancia(distancia, datoActual.texto, datoActual.descripcion));
				}

				foreach (var puntero in arbol.getHijos())
				{
					if (puntero.getDatoRaiz().distancia >= distanciaMinima && puntero.getDatoRaiz().distancia <= distanciaMaxima)
					{
						Buscar(puntero, elementoABuscar, umbral, collected);
					}
				}

			}
		}

		private void busquedaHojas(ArbolGeneral<DatoDistancia> arbol, List<string> hojasEncontradas)
		{
			if (arbol.esHoja() == true)
			{
				hojasEncontradas.Add(arbol.getDatoRaiz().texto);
			}
			else
			{
				foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
				{
					busquedaHojas(hijo, hojasEncontradas);
				}
			}
		}
		private void recoleccionCaminos(ArbolGeneral<DatoDistancia> arbol,string caminoActual, List<string> caminosEncontrados)
		{
			if (arbol.esHoja() == true)
			{
				caminosEncontrados.Add(caminoActual);
			}
			else
			{
				foreach (ArbolGeneral<DatoDistancia> hijo in arbol.getHijos())
				{
					string caminoActualizado= caminoActual + " ---> " + hijo.getDatoRaiz().texto;
					recoleccionCaminos(hijo,caminoActualizado,caminosEncontrados);
                }
			}
		}
    }
}