using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShadowModule : PlayerModule
{
    private struct RendererVisibilityState
    {
        public Renderer Renderer;
        public bool WasEnabled;
    }

    private bool shadowActive;
    private float timeSinceUse;
    private readonly float cooldownTime;
    private readonly float activeDuration;
    private Coroutine shadowRoutine;
    private GameObject shadowProjectorInstance;
    private GameObject reconstructionParticleInstance;

    private readonly List<RendererVisibilityState> rendererVisibilityStates = new List<RendererVisibilityState>();
    private bool hasCachedRendererVisibility;

    public readonly ShadowModuleSO base_data;

    public ShadowModule(
        PlayerController player,
        PlayerModuleSO base_data_so
    ) : base(player, base_data_so)
    {
        base_data = (ShadowModuleSO)base_data_so;
        cooldownTime = base_data.cooldownTime;
        activeDuration = base_data.activeDuration;
    }

    public override void FixedUpdateModule()
    {
        UpdateProjectorPosition();
        UpdateReconstructionVFXPosition();
    }

    public override void UpdateModule()
    {
        if (player.player_ability == null || !player.player_ability.WasPressedThisFrame())
        {
            return;
        }

        if (shadowActive)
        {
            ReleaseModule(default);
            return;
        }

        UseModule(default);
    }

    public override void OnDeactivate()
    {
        ReleaseModule(default);
        DestroyProjector();
    }

    public virtual void ReleaseModule(InputAction.CallbackContext context)
    {
        PlayReconstructionVFX();
        if (shadowRoutine != null)
        {
            player.StopCoroutine(shadowRoutine);
            shadowRoutine = null;
        }

        shadowActive = false;
        RestoreRendererVisibility();
        SetProjectorActive(false);
        player.tag = "Player";
    }

    public override void UseModule(InputAction.CallbackContext context)
    {
        if (shadowActive)
        {
            return;
        }

        shadowActive = true;
        timeSinceUse = 0f;
        HidePlayerRenderers();
        EnsureProjectorInstance();
        SetProjectorActive(true);
        UpdateProjectorPosition();
        shadowRoutine = player.StartCoroutine(ShadowDuration());
        player.tag = "ShadowPlayer";
    }

    public float GetShadowChargePerc()
    {
        if (cooldownTime <= 0f)
        {
            return 0f;
        }

        return timeSinceUse / cooldownTime;
    }

    private IEnumerator ShadowDuration()
    {
        while (timeSinceUse < activeDuration)
        {
            timeSinceUse += Time.deltaTime;
            yield return null;
        }

        ReleaseModule(default);
    }

    private void HidePlayerRenderers()
    {
        if (!hasCachedRendererVisibility)
        {
            rendererVisibilityStates.Clear();
            Renderer[] renderers = player.GetComponentsInChildren<Renderer>(true);
            for (int i = 0; i < renderers.Length; i++)
            {
                Renderer renderer = renderers[i];
                if (renderer == null)
                {
                    continue;
                }

                if (!(renderer is MeshRenderer) && !(renderer is SkinnedMeshRenderer))
                {
                    continue;
                }

                rendererVisibilityStates.Add(new RendererVisibilityState
                {
                    Renderer = renderer,
                    WasEnabled = renderer.enabled
                });
            }

            hasCachedRendererVisibility = rendererVisibilityStates.Count > 0;
        }

        for (int i = 0; i < rendererVisibilityStates.Count; i++)
        {
            RendererVisibilityState state = rendererVisibilityStates[i];
            if (state.Renderer == null)
            {
                continue;
            }

            state.Renderer.enabled = false;
        }
    }

    private void RestoreRendererVisibility()
    {
        if (!hasCachedRendererVisibility)
        {
            return;
        }

        for (int i = 0; i < rendererVisibilityStates.Count; i++)
        {
            RendererVisibilityState state = rendererVisibilityStates[i];
            if (state.Renderer == null)
            {
                continue;
            }

            state.Renderer.enabled = state.WasEnabled;
        }

        rendererVisibilityStates.Clear();
        hasCachedRendererVisibility = false;
    }

    private void EnsureProjectorInstance()
    {
        if (shadowProjectorInstance != null)
        {
            return;
        }

        if (base_data.shadowProjectorPrefab == null)
        {
            return;
        }

        shadowProjectorInstance = Object.Instantiate(base_data.shadowProjectorPrefab);
        shadowProjectorInstance.name = base_data.shadowProjectorPrefab.name + "_Runtime";
        shadowProjectorInstance.SetActive(false);
    }

    private void SetProjectorActive(bool shouldBeActive)
    {
        if (shadowProjectorInstance == null)
        {
            return;
        }

        if (shadowProjectorInstance.activeSelf != shouldBeActive)
        {
            shadowProjectorInstance.SetActive(shouldBeActive);
        }
    }

    private void UpdateProjectorPosition()
    {
        if (!shadowActive || shadowProjectorInstance == null)
        {
            return;
        }

        shadowProjectorInstance.transform.position = player.transform.position + base_data.shadowProjectorOffset;
    }

    private void DestroyProjector()
    {
        if (shadowProjectorInstance == null)
        {
            return;
        }

        Object.Destroy(shadowProjectorInstance);
        shadowProjectorInstance = null;
    }

    private void PlayReconstructionVFX()
    {
        if (base_data.reconstructionParticlePrefab == null)
        {
            return;
        }

        if (reconstructionParticleInstance != null)
        {
            Object.Destroy(reconstructionParticleInstance);
        }

        reconstructionParticleInstance = Object.Instantiate(
            base_data.reconstructionParticlePrefab,
            player.transform.position,
            Quaternion.identity
        );
    }

    private void UpdateReconstructionVFXPosition()
    {
        if (reconstructionParticleInstance == null)
        {
            return;
        }

        reconstructionParticleInstance.transform.position = player.transform.position;
    }
}
