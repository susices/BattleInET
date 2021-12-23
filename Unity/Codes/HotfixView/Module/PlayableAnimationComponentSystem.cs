using Animancer;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;
namespace ET.Module
{
    public class AnimationComponentAwakeSystem: AwakeSystem<AnimationComponent>
    {
        public override void Awake(AnimationComponent self)
        {
            var gameObjectComponent = self.Parent.GetComponent<GameObjectComponent>();
            if (gameObjectComponent==null)
            {
                Log.Error("无法找到GameObjectComponent组件");
                self.Dispose();
                return;
            }

            var animator = gameObjectComponent.GameObject.GetComponent<Animator>();
            if (animator==null)
            {
                Log.Error("无法找到Animator组件");
                self.Dispose();
                return;
            }

            var AnimancerComponent = gameObjectComponent.GameObject.GetComponent<AnimancerComponent>();
            if (self.AnimancerComponent==null)
            {
                AnimancerComponent = gameObjectComponent.GameObject.AddComponent<AnimancerComponent>();
            }
            self.Animator = animator;
            self.AnimancerComponent = AnimancerComponent;
            
            var clip = GlobalComponent.Instance.Global.GetComponent<ReferenceCollector>().Get<AnimationClip>("Attack");
            
        }
    }
    
    public class AnimationComponentDestorySystem: DestroySystem<AnimationComponent>
    {
        public override void Destroy(AnimationComponent self)
        {
            self.Animator = null;
            self.AnimancerComponent = null;
        }
    }

    public static class AnimationComponentSystem
    {
        public static void Play(this AnimationComponent self, AnimationClip clip, float fadeTime = 0.25f, FadeMode fadeMode = FadeMode.FixedSpeed)
        {
            var animancerState =  self.AnimancerComponent.Play(clip, fadeTime, fadeMode);
        }
        
        
    }
}