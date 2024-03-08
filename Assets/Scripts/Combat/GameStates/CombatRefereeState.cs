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

            yield return new RoundEndState(this).Execute();

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
            List<CombatEntity> team = _stateHandler.Referee.GetCurrentTeam();

  
            //1.) Prepare current team for Enter Turn of the entity, this included running status effect 
            foreach (CombatEntity entity in team)
            {
                yield return entity.TurnEnter();     
            }

            //2.) Prepare Referee for new round 
            yield return _stateHandler.Referee.PrepareRefereeForNewRound();

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

            ActorAction action ; 
            foreach (CombatEntity actor in team)
            {
                yield return actor.GetAilmentAction(); 

                if ((action = actor.GetActionRuntimeEffect()) != null)
                {
                    yield return _stateHandler.Referee.OnCharacterPerformAction(actor, action);
                }
            }
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

            ActorAction action = null ;

            _stateHandler.Referee.SetActiveActors(); 

            while (true)
            {
                
                _actor = _stateHandler.Referee.GetCurrentActor() ;

                if (_actor == null)
                    break; 
                 
                if ((action = _actor.GetActionRuntimeEffect()) == null)  
                    yield return _actor.GetAction() ;

                else
                {
                    yield return _stateHandler.Referee.OnCharacterPerformAction(_actor,action);
                    
                    break; 
                }    
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

    public class PostPerformActionState : CombatRefereeState
    {
        public PostPerformActionState(CombatRefereeStateHandler handler) : base(handler)
        {

        }

        public override IEnumerator Execute()
        {
            //We check if there is any character who want to do action
            

            yield return new PerformActionState(_stateHandler).Execute() ; 
             
        }

        private List<CombatEntity> QuerryForEntityWithReaction()
        {
            //Querry for player first 
            foreach (CombatEntity entity in _stateHandler.Referee.GetCompetatorsBySide(ECompetatorSide.Ally))
            {
                
            }
            throw new NotImplementedException(); 
            return new List<CombatEntity>() ;
            //Querry for enemy side
        }

        protected override IEnumerator StateEnter()
        {
            yield return null;
        }

        protected override IEnumerator StateExit()
        {
            yield return null;
        }
    }
    public class RoundEndState : CombatRefereeState
    {
        public RoundEndState(CombatRefereeStateHandler handler) : base(handler)
        {

        }

        public override IEnumerator Execute()
        {
            yield return StateEnter();

            yield return StateExit();
        }

        protected override IEnumerator StateEnter()
        {
            List<CombatEntity> team = _stateHandler.Referee.GetCurrentTeam();

            //1.) Call Enter Turn of the entity, this included running status effect 
            foreach (CombatEntity entity in team)
            {
                yield return entity.TurnLeave() ;
            }
        }

        protected override IEnumerator StateExit()
        {
            yield return null;
        }
    }

    #endregion


}
