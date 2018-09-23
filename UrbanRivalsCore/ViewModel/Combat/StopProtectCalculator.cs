using System;
using UrbanRivalsCore.Model;

namespace UrbanRivalsCore.ViewModel
{
    internal class StopProtectLink
    {
        public bool isLeft;
        public bool inspected;
        public StopProtectLink Prev;
        public StopProtectLink Next;
        public ActivationStatus Status = ActivationStatus.Normal;
        public ActivationCases Action = ActivationCases.NoChain;

        public StopProtectLink() { }
    }
    internal static class StopProtectCalculator
    {
        public static void CalculateAndApplyStopProtectChains(CombatRoundSkillsHandler skillsHandler)
        {
            ActivationStatus[] status = new ActivationStatus[4];
            status[(int)SkillIndex.LA] = skillsHandler.GetStatus(SkillIndex.LA);
            status[(int)SkillIndex.LB] = skillsHandler.GetStatus(SkillIndex.LB);
            status[(int)SkillIndex.RA] = skillsHandler.GetStatus(SkillIndex.RA);
            status[(int)SkillIndex.RB] = skillsHandler.GetStatus(SkillIndex.RB);

            status = CalculateStopProtectChains(status,
                skillsHandler[SkillIndex.LA], skillsHandler[SkillIndex.RA],
                skillsHandler[SkillIndex.LB], skillsHandler[SkillIndex.RB]);

            for (int i = 0; i < 4; i++)
            {
                if (status[i] == ActivationStatus.Normal)
                    skillsHandler.UnfreezeSkill(i);
                else
                    skillsHandler.FreezeSkill(i);
            }
        }
        public static ActivationStatus[] CalculateStopProtectChains(ActivationStatus[] initialActivationStatus, Skill leftAbility, Skill rightAbility, Skill leftBonus, Skill rightBonus)
        {
            StopProtectLink[] Links = GenerateInitialChainLinks(initialActivationStatus, leftAbility, rightAbility, leftBonus, rightBonus);

            // If there are 4 stops, or none, we can end this right here
            if (Links[(int)SkillIndex.LA].Action.HasFlag(ActivationCases.Stop) &&
                Links[(int)SkillIndex.RA].Action.HasFlag(ActivationCases.Stop) &&
                Links[(int)SkillIndex.LB].Action.HasFlag(ActivationCases.Stop) &&
                Links[(int)SkillIndex.RB].Action.HasFlag(ActivationCases.Stop))
                return CreateActivationStatusArray(ActivationStatus.Stopped);

            if (!Links[(int)SkillIndex.LA].Action.HasFlag(ActivationCases.Stop) &&
                !Links[(int)SkillIndex.RA].Action.HasFlag(ActivationCases.Stop) &&
                !Links[(int)SkillIndex.LB].Action.HasFlag(ActivationCases.Stop) &&
                !Links[(int)SkillIndex.RB].Action.HasFlag(ActivationCases.Stop))
                return CreateActivationStatusArray(ActivationStatus.Normal);

            SetStopChains(Links);

            ApplyStops(Links);

            ApplyProtects(Links);

            ActivationStatus[] result = new ActivationStatus[4];
            for (int i = 0; i < 4; i++)
                result[i] = Links[i].Status;

            return result;
        }
        private static ActivationStatus[] CreateActivationStatusArray(ActivationStatus status)
        {
            ActivationStatus[] result = new ActivationStatus[4];
            for (int i = 0; i < 4; i++)
                result[i] = status;
            return result;
        }
        private static ActivationCases DecodeActivationCases(Skill skill)
        {
            switch (skill.Suffix)
            {
                case SkillSuffix.StopAbility:
                    return ActivationCases.StopAbility | ActivationCases.Stop;
                case SkillSuffix.StopBonus:
                    return ActivationCases.StopBonus | ActivationCases.Stop;
                case SkillSuffix.ProtectAbility:
                    return ActivationCases.ProtectAbility | ActivationCases.Protect;
                case SkillSuffix.ProtectBonus:
                    return ActivationCases.ProtectBonus | ActivationCases.Protect;
                default:
                    return ActivationCases.NoChain;
            }
        }
        private static StopProtectLink[] GenerateInitialChainLinks(ActivationStatus[] initialActivationStatus, Skill leftAbility, Skill rightAbility, Skill leftBonus, Skill rightBonus)
        {
            StopProtectLink[] Links = new StopProtectLink[4];
            for (int i = 0; i < 4; i++)
                Links[i] = new StopProtectLink();
            Links[(int)SkillIndex.LA].isLeft = true;
            Links[(int)SkillIndex.LB].isLeft = true;

            // Store previous status
            for (int i = 0; i < 4; i++)
                Links[i].Status = initialActivationStatus[i];

            // Store actions
            if (Links[(int)SkillIndex.LA].Status == ActivationStatus.Normal)
                Links[(int)SkillIndex.LA].Action = DecodeActivationCases(leftAbility);
            if (Links[(int)SkillIndex.RA].Status == ActivationStatus.Normal)
                Links[(int)SkillIndex.RA].Action = DecodeActivationCases(rightAbility);
            if (Links[(int)SkillIndex.LB].Status == ActivationStatus.Normal)
                Links[(int)SkillIndex.LB].Action = DecodeActivationCases(leftBonus);
            if (Links[(int)SkillIndex.RB].Status == ActivationStatus.Normal)
                Links[(int)SkillIndex.RB].Action = DecodeActivationCases(rightBonus);

            return Links;
        }
        private static StopProtectLink[] ResetInspectedState(StopProtectLink[] links)
        {
            for (int i = 0; i < 4; i++)
                links[i].inspected = false;
            return links;
        }
        private static void SetStopChains(StopProtectLink[] links)
        {
            // Sets the chains of stops, we will check protects at the end
            foreach (StopProtectLink link in links)
            {
                if (link.inspected)
                    continue;

                if (!link.Action.HasFlag(ActivationCases.Stop))
                    continue;

                if (link.Action.HasFlag(ActivationCases.StopAbility))
                    link.Next = (link.isLeft) ? links[(int)SkillIndex.RA] : links[(int)SkillIndex.LA];
                else if (link.Action.HasFlag(ActivationCases.StopBonus))
                    link.Next = (link.isLeft) ? links[(int)SkillIndex.RB] : links[(int)SkillIndex.LB];

                link.Next.Prev = link;
                link.inspected = true;
            }

            ResetInspectedState(links);
        }
        private static void ApplyStops(StopProtectLink[] links)
        {
            // Mark as Stopped any Skill that is the target of other Stop
            foreach (StopProtectLink link in links)
            {
                if (link.inspected)
                    continue;

                // Go to the head of the queue
                int i = 0;
                StopProtectLink head = link;
                bool infiniteLoop = false;
                while (head.Prev != null)
                {
                    head = head.Prev;
                    if (i++ >= 2) // 4 stops loop was already checked with the initial IF, so the maximum loop is 3 stops
                    {
                        infiniteLoop = true;
                        break;
                    }
                }

                if (infiniteLoop)
                {
                    for (i = 0; i < 3; i++) // 4 stops loop was already checked with the initial IF, so the maximum loop is 3 stops
                    {
                        head.Status = ActivationStatus.Stopped;
                        head = head.Next;
                    }
                }
                else
                {
                    while (!(head == null || head.inspected))
                    {
                        head.inspected = true;
                        if (!head.Status.HasFlag(ActivationStatus.Stopped))
                            if (head.Action.HasFlag(ActivationCases.Stop))
                                head.Next.Status = ActivationStatus.Stopped;
                        head = head.Next;
                    }
                }
            }
        }
        private static void ApplyProtects(StopProtectLink[] links)
        {
            // Any Protect Skill that is not already Stopped cleans the stop from its target
            foreach (StopProtectLink link in links)
            {
                if (link.Status == ActivationStatus.Stopped)
                    continue;

                if (!link.Action.HasFlag(ActivationCases.Protect))
                    continue;

                if (link.Action.HasFlag(ActivationCases.ProtectAbility))
                {
                    if (link.isLeft)
                        links[(int)SkillIndex.LA].Status = ActivationStatus.Normal;
                    else
                        links[(int)SkillIndex.RA].Status = ActivationStatus.Normal;
                }
                else if (link.Action.HasFlag(ActivationCases.ProtectBonus))
                {
                    if (link.isLeft)
                        links[(int)SkillIndex.LB].Status = ActivationStatus.Normal;
                    else
                        links[(int)SkillIndex.RB].Status = ActivationStatus.Normal;
                }
            }
        }
    }
}