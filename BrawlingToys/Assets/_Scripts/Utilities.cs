using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    /*
    - AddForce: ForceMode.Impulse

        1) Ponto inicial de aplica��o da for�a
        2) Dire��o 
        3) Uma taxa de atualiz���o da for�a no momento da aplica��o
        4) Taxa de movimento resultante ap�s a for�a parar de ser aplicada
        5) ForceMode?*

        NOTA_1 : Como n�o teremos Update aqui, n�o podemos trabalhar com a classe Time. Portanto, o "instante" que � aplicada uma for�a
               ser� traduzido em quantas unidades de espa�o voce quer que aquele objeto se desloque at� concluir o movimento;

        NOTA_2 : Como n�o estamos trabalhando com Rigidbody, n�o ter� o conceito de massa de um objeto. Nesse caso, o metodo funicona 
               como um VelocityChange.
    */

    public static void AddForce(Vector3 incialPosition, Vector3 finalPosition)
    {

    }
}
