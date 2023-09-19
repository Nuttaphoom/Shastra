using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Vanaring 
{   
    public class CombatRefereeStateHandler
    {
        private CombatReferee _referee ;

        public CombatRefereeStateHandler(CombatReferee referee) { 
            this._referee = referee;
        }

        #region Public_Methods 
        public IEnumerator AdvanceRound()
        {
            yield return new RoundEnterState(this).Execute();

            while (_referee.GetCurrentActiveEntities().Count > 0)
            {
                yield return AdvanceTurn();
            }
        }


        /// <summary>
        /// AdvanceTurn is mostly used for PerformingAction Loop
        /// Ran until the "turn" end
        /// </summary>
        /// <returns></returns>
        private IEnumerator AdvanceTurn()
        {
            //Perform Action
            yield return new PerformActionState(this).Execute() ; 

        }

        #endregion

        #region GETTER
        public CombatReferee Referee { get { return _referee; } }
        #endregion
    }

    public abstract class CombatRefereeState
    {
        protected CombatRefereeStateHandler _stateHandler  ;  
        public CombatRefereeState(CombatRefereeStateHandler handler)
        {
            this._stateHandler = handler; 
        }
        protected abstract IEnumerator StateEnter() ;
        protected abstract IEnumerator StateExit() ;

        /// <summary>
        /// Execute() differs from Update() in that it should only be invoked once during the lifetime of a State.
        /// </summary>
        /// <returns></returns>
        public abstract IEnumerator Execute();

     }

    #region States 

    public class RoundEnterState : CombatRefereeState
    { 
        public RoundEnterState(CombatRefereeStateHandler handler) : base(handler) 
        {
        
        }
        public override IEnumerator Execute()
        {
            yield return StateEnter();

            yield return StateExit(); 
        }

        protected override IEnumerator StateEnter()
        {
            //Prepare Referee for new round 
            yield return _stateHandler.Referee.PrepareRefereeForNewRound();  
             
            List<CombatEntity> team = _stateHandler.Referee.GetCurrentTeam();

            //1.) Call Enter Turn of the entity, this included running status effect 
            foreach (CombatEntity entity in team)
            {
                yield return entity.TurnEnter();     
            }
            
            ////2.) Notify UI elements
            //if (team.Count > 0)
            //{
            //    for (int i = 0; i < team.Count; i++)
            //    {
            //        MonoBehaviour.FindObjectOfType<CharacterWindowManager>().SetHighlightActiveEntity(team[i]);
            //    }
            //    _currentEntityIndex = 0;
            //    yield return SwitchControl(-1, _currentEntityIndex);
            //}
        }

        protected override IEnumerator StateExit()
        {
            yield return null; 
        }
    }

    public class PerformActionState : CombatRefereeState
    {
         
        public PerformActionState(CombatRefereeStateHandler handler) : base(handler)
        {

        }

        public override IEnumerator Execute()
        {
            CombatEntity _actor; 

            RuntimeEffect action = null ; 

            while (true)
            {
                //Asking for actor every frame
                _actor = _stateHandler.Referee.GetCurrentActor() ;

                //No Actor left (Possibly, everyone is exhaunted) 
                if (_actor == null)
                    break; 
                 
                yield return _actor.GetAction();

                if ( (action = _actor.GetActionRuntimeEffect()  ) != null)
                {
                    //Maybe it get overheat or some affect stunt it while controling 
                    if (_actor.ReadyForControl())
                    {
                        //ExecuteAction 
                        yield return _actor.OnPerformAction(action); 
                    }
                    break; 
                }
                else
                    yield return new WaitForEndOfFrame(); 
            }
        }

        protected override IEnumerator StateEnter()
        {
            throw new NotImplementedException();
        }

        protected override IEnumerator StateExit()
        {
            throw new NotImplementedException();
        }
    }


    #endregion


}
