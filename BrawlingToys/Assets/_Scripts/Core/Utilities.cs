using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities 
{
    /*
    - AddForce: ForceMode.Impulse

        1) Ponto inicial de aplicação da força
        2) Direção 
        3) Uma taxa de atualizãção da força no momento da aplicação
        4) Taxa de movimento resultante após a força parar de ser aplicada
        5) ForceMode?*

        NOTA_1 : Como não teremos Update aqui, não podemos trabalhar com a classe Time. Portanto, o "instante" que é aplicada uma força
               será traduzido em quantas unidades de espaço voce quer que aquele objeto se desloque até concluir o movimento;

        NOTA_2 : Como não estamos trabalhando com Rigidbody, não terá o conceito de massa de um objeto. Nesse caso, o metodo funicona 
               como um VelocityChange.
    */

    public static void AddForce(Vector3 incialPosition, Vector3 finalPosition)
    {

    }
}
