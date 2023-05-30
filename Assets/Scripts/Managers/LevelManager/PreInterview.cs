using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class PreInterview : IState
{
    private Option my_option;
    private LevelManager me;
    public UnityEvent method; 

    private bool finish;
    private int stage = 0;

    public PreInterview(LevelManager level_manager)
    {
        this.me = level_manager;
    }

    public void EnterState()
    {
        /*
        switch (Singleton.Instance.current_day)
        {
            case 1:
                method = new UnityEvent();
                method.AddListener(Level1);
                break;
        }*/
    }

    public void UpdateState()
    {
        switch (Singleton.Instance.current_day)
        {
            case 1: day_1(); break;
            case 2: day_2(); break;
            case 3: day_3(); break;
            default: finish = true; break;
        }

        if (finish)
        {
            me.change_state(me.person_intro);
        }
    }

    public void ExitState()
    {
        
    }

    public void day_1()
    {
        switch (stage)
        {
            case 0:
            {
                var in_room = me.create_person(Singleton.Instance.manager_sprite, Singleton.Instance.manager_animation);
                me.current_person.dialogue_character.person_in_room = in_room;

                stage++;
            }
            break;
            case 1:
            {
                if (me.current_person_obj.HasEntered())
                {
                    DialogueStruc[] dialogue = {
                        new DialogueStruc("Bom dia, você deve ser o novato, certo?",Singleton.Instance.manager_character),
                        new DialogueStruc("Meu nome é Natasha, sou a gerente de RH dessa filial.",Singleton.Instance.manager_character),
                        new DialogueStruc("Permita-me explicar como você trabalhará pelos próximos 3 dias.",Singleton.Instance.manager_character),
                        new DialogueStruc("Todo dia, você receberá uma lista de candidatos para entrevistar. Destes, você deve contratar 2. Você pode exceder esse número, mas evite faze-lo.",Singleton.Instance.manager_character),
                        new DialogueStruc("Antes de cada entrevista, você receberá curriculo do entrevistado. Assim que terminar de ler o currículo, a entrevista terá início, faça perguntas para tentar entender o perfil do individuo, em qual cargo ele se encaixaria melhor.",Singleton.Instance.manager_character),
                        new DialogueStruc("Alguns candidatos mentirão sobre suas competências, no curriculo, então fique atento e teste suas habilidades de vez em quando.",Singleton.Instance.manager_character),
                        new DialogueStruc("Informações mais técnicas sobre seu trabalho estarão na pasta ao lado, fiz o favor de grifar informações importantes.",Singleton.Instance.manager_character),
                        new DialogueStruc("Lhe restou alguma dúvida?",Singleton.Instance.manager_character)};

                    me.create_dialogue(dialogue);

                    stage++;
                }
            }
            break;
            case 2:
            {
                if (me.current_dialogue.finished_text())
                {
                    OptionValues[] current_options = {
                        new OptionValues("Sim", new Person.Question()),
                        new OptionValues("Não", new Person.Question())};
                    
                    my_option = me.create_options(current_options);
                    stage++;
                }
            }
            break;
            case 3:
            {
                if (my_option.choosen_option!=-1)
                {
                    if (my_option.choosen_option_struc.text == "Sim")
                    {
                        DialogueStruc[] dialogue = {
                        new DialogueStruc("Todo dia, você receberá uma lista de candidatos para entrevistar. Destes, você deve contratar 2. Você pode exceder esse número, mas evite faze-lo.",Singleton.Instance.manager_character),
                        new DialogueStruc("Antes de cada entrevista, você receberá curriculo do entrevistado. Assim que terminar de ler o currículo, a entrevista terá início, faça perguntas para tentar entender o perfil do individuo, em qual cargo ele se encaixaria melhor.",Singleton.Instance.manager_character),
                        new DialogueStruc("Alguns candidatos mentirão sobre suas competências, no curriculo, então fique atento e teste suas habilidades de vez em quando.",Singleton.Instance.manager_character),
                        new DialogueStruc("Informações mais técnicas sobre seu trabalho estarão na pasta ao lado, fiz o favor de grifar informações importantes.",Singleton.Instance.manager_character),
                        new DialogueStruc("Lhe restou alguma dúvida?",Singleton.Instance.manager_character)};

                        me.set_current_dialogue(dialogue);
                        stage--;
                    }
                    else
                    {
                        DialogueStruc[] dialogue = {
                        new DialogueStruc("Ótimo...",Singleton.Instance.manager_character),
                        new DialogueStruc("Ao final de cada dia, nosso algoritimo Future irá calcular sua performance, e lhe dar um bonus em FP com base nesta.",Singleton.Instance.manager_character),
                        new DialogueStruc("FP, Future Points, são o novo sistema da Future para recompensar bons colaboradores. Faça um bom trabalho e ganhará algumas centenas deles.",Singleton.Instance.manager_character),
                        new DialogueStruc("Ao final do terceiro dia. Decidiremos seu futuro na empresa com base em seu FP.",Singleton.Instance.manager_character),
                        new DialogueStruc("Boa sorte... E lembre-se do nosso bordão, O futuro é agora!",Singleton.Instance.manager_character)};

                        me.set_current_dialogue(dialogue);
                        me.current_dialogue.hide_at_end = true;

                        stage++;
                    }
                }
            }
            break;
            case 4:
            {
                if (me.current_dialogue.hiding_box())
                {
                    me.destroy_dialogue();
                    me.current_dialogue = null;
                    me.current_options = null;
                    me.current_person_obj.exiting = true;

                    stage++;
                }
            }
            break;
            case 5:
            {
                if (me.current_person_obj.HasExited())
                {
                    me.destroy_person_obj();
                    me.current_person_obj = null;
                    finish = true;

                    stage++;
                }
            }
            break;
        }
    }

    public void day_2()
    {
        switch (stage)
        {
            case 0:
            {
                var in_room = me.create_person(Singleton.Instance.manager_sprite, Singleton.Instance.manager_animation);
                me.current_person.dialogue_character.person_in_room = in_room;

                stage++;
            }
            break;
            case 1:
            {
                if (me.current_person_obj.HasEntered())
                {
                    if (Singleton.Instance.future_points > 0)
                    {
                        DialogueStruc[] dialogue = {
                        new DialogueStruc("Bom dia novato, o dia de ontem foi corrido, não foi?",Singleton.Instance.manager_character),
                        new DialogueStruc("Você não tem nem idéia.",Singleton.Instance.player_character),
                        new DialogueStruc("Eu consigo imaginar. Mas está fazendo um bom trabalho. Continue assim e talvez seja efetivado.",Singleton.Instance.manager_character),
                        new DialogueStruc("Bom... Não tenho nada a adicionar hoje, não vou gastar mais do seu tempo, bom trabalho.",Singleton.Instance.manager_character)};

                        me.create_dialogue(dialogue);
                    }
                    else
                    {
                        DialogueStruc[] dialogue = {
                        new DialogueStruc("Ola novato, acompanhei seu trabalho ontem.",Singleton.Instance.manager_character),
                        new DialogueStruc("Não posso dizer que foi decepcionante, pois já não tinha muita expectativa em você. Acho que mediocre é a palavra certa...",Singleton.Instance.manager_character),
                        new DialogueStruc("Espero que sua performance seja melhor hoje... A Future não aceita colaboradores incompetentes.",Singleton.Instance.manager_character)};
                        
                        me.create_dialogue(dialogue);
                    }
                    

                    
                    me.current_dialogue.hide_at_end = true;
                    stage++;
                }
            }
            break;
            case 2:
            {
                if (me.current_dialogue.hiding_box())
                {
                    me.destroy_dialogue();
                    me.current_dialogue = null;
                    me.current_options = null;
                    me.current_person_obj.exiting = true;

                    stage++;
                }
            }
            break;
            case 3:
            {
                if (me.current_person_obj.HasExited())
                {
                    me.destroy_person_obj();
                    me.current_person_obj = null;
                    finish = true;

                    stage++;
                }
            }
            break;
        }
    }

    public void day_3()
    {
        switch (stage)
        {
            case 0:
            {
                var in_room = me.create_person(Singleton.Instance.manager_sprite, Singleton.Instance.manager_animation);
                me.current_person.dialogue_character.person_in_room = in_room;

                stage++;
            }
            break;
            case 1:
            {
                if (me.current_person_obj.HasEntered())
                {
                    DialogueStruc[] dialogue = {
                    new DialogueStruc("Bom dia novato, você deve ter visto nas notícias, mas ococrreu um roubo no armazem da filial na noite passada...",Singleton.Instance.manager_character),
                    new DialogueStruc("Estamos tentando reforçar a segurança pra lidar com este tipo de crime... Faça questão de contratar pelo menos um segurança hoje",Singleton.Instance.manager_character)};

                    me.create_dialogue(dialogue);                    
                    
                    me.current_dialogue.hide_at_end = true;
                    stage++;
                }
            }
            break;
            case 2:
            {
                if (me.current_dialogue.hiding_box())
                {
                    me.destroy_dialogue();
                    me.current_dialogue = null;
                    me.current_options = null;
                    me.current_person_obj.exiting = true;

                    stage++;
                }
            }
            break;
            case 3:
            {
                if (me.current_person_obj.HasExited())
                {
                    me.destroy_person_obj();
                    me.current_person_obj = null;
                    finish = true;

                    stage++;
                }
            }
            break;
        }
    }
}